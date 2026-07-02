using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Renderers;

/*
 * CrosshatchTextureLayer — design notes
 * ======================================
 *
 * TECHNIQUE
 * ----------
 * No noise is involved. The texture is built entirely from geometry: two (or
 * one) families of parallel lines drawn at ±Angle degrees. For each pixel we
 * compute how far it sits from the nearest line in each family; if that
 * distance is within LineWidth/2 the pixel is "ink", otherwise it is "paper".
 *
 * True perpendicular distance from a pixel to the nearest line is found by
 * projecting onto the line's unit normal (cos θ, sin θ):
 *
 *   projection = x·cos θ + y·sin θ
 *   gap        = min(projection % Spacing, Spacing − projection % Spacing)
 *
 * Because (cos θ, sin θ) is already a unit vector, gap is the exact
 * perpendicular distance in pixels — no correction factor is needed or correct.
 *
 * ANTI-ALIASING
 * --------------
 * A hard gap <= halfLine threshold produces uneven apparent thickness across
 * angles because different angles sample pixel centres more or less favourably.
 * For example, a 45° line clips pixel corners while a 0° line clips whole
 * columns, so the 45° line looks thinner even though the geometry is correct.
 *
 * The fix is to replace the hard threshold with a smooth blend over a 1-pixel
 * transition zone on each side of the line edge:
 *
 *   t = smoothstep(halfLine + 0.5, halfLine - 0.5, gap)
 *
 * Pixels well inside the line (gap << halfLine) get t ≈ 1 (full ink).
 * Pixels well outside (gap >> halfLine) get t ≈ 0 (full paper).
 * Pixels on the edge blend proportionally, eliminating the staircase effect
 * and making all angles read as the same visual weight.
 *
 * SECOND PASS
 * ------------
 * If SecondAngle is set, a second family of lines is added at that angle.
 * Coverage is accumulated — a pixel touched by both families gets full ink.
 *
 * NOISE JITTER
 * -------------
 * An optional JitterAmount perturbs each pixel's lookup position by a small
 * noise value before the line test. This breaks the mechanical regularity and
 * gives the texture a hand-drawn, etching-like quality.
 */

/// <summary>
/// Renders a crosshatch (parallel or grid) ink-on-paper texture.
/// Purely geometric — no noise required, though optional jitter is supported.
/// See the block comment above for a full explanation of the technique.
/// </summary>
public sealed class CrosshatchTextureLayer : CachedBitmapLayer
{
    /// <summary>
    /// Amount that hatch lines bend. 0 = perfectly straight.
    /// 1-3 gives a subtle hand-drawn curve.
    /// </summary>
    public double WaveAmplitude { get; set; } = 2.0;

    /// <summary>
    /// Scale of the line waviness. Smaller = longer waves.
    /// </summary>
    public double WaveFrequency { get; set; } = 0.025;

    /// <summary>Angle of the first line family, in degrees.</summary>
    public double Angle { get; set; } = 45.0;

    /// <summary>
    /// Angle of the second line family, in degrees.
    /// Set to null to draw only one family of parallel lines.
    /// </summary>
    public double? SecondAngle { get; set; } = 135.0; // 45 + 90 → classic crosshatch

    /// <summary>Distance between line centres, in pixels.</summary>
    public double Spacing { get; set; } = 12.0;

    /// <summary>Thickness of each line, in pixels.</summary>
    public double LineWidth { get; set; } = 1.2;

    /// <summary>
    /// How much noise displaces the line-lookup position.
    /// 0 = perfectly mechanical; ~2–4 gives a hand-drawn feel.
    /// </summary>
    public double JitterAmount { get; set; } = 0.0;

    /// <summary>Paper (background) colour.</summary>
    public Color PaperColour { get; set; } = Color.FromArgb(255, 245, 240, 228); // warm ivory

    /// <summary>Ink (line) colour.</summary>
    public Color InkColour { get; set; } = Color.FromArgb(255, 30, 25, 20); // near-black

    public override int Order => 0;

    protected override void Generate(
        WriteableBitmap bitmap,
        RenderContext context)
    {
        int w = bitmap.PixelWidth;
        int h = bitmap.PixelHeight;
        int stride = w * 4;
        byte[] pixels = new byte[h * stride];

        // Pre-compute trig for both line families.
        double rad1 = Angle * Math.PI / 180.0;
        double cos1 = Math.Cos(rad1);
        double sin1 = Math.Sin(rad1);

        bool hasSec = SecondAngle.HasValue;
        double cos2 = 0, sin2 = 0;
        if (hasSec)
        {
            double rad2 = SecondAngle!.Value * Math.PI / 180.0;
            cos2 = Math.Cos(rad2);
            sin2 = Math.Sin(rad2);
        }

        double halfLine = LineWidth / 2.0;
        bool useJitter = JitterAmount > 0.0 && context.Noise != null;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                double px = x;
                double py = y;

                if (useJitter)
                {
                    double jitter = context.Noise.Fractal(x * 0.05, y * 0.05, 3)
                                    * JitterAmount;
                    px += jitter;
                    py += jitter;
                }
                double coverage = LineCoverage(
                    px, py,
                    cos1, sin1,
                    Spacing,
                    halfLine,
                    context,
                    WaveAmplitude,
                    WaveFrequency);

                if (hasSec)
                {
                    coverage = Math.Max(
                        coverage,
                        LineCoverage(
                            px, py,
                            cos2, sin2,
                            Spacing,
                            halfLine,
                            context,
                            WaveAmplitude,
                            WaveFrequency));
                }

                // Blend paper → ink by coverage.
                byte r = Blend(PaperColour.R, InkColour.R, coverage);
                byte g = Blend(PaperColour.G, InkColour.G, coverage);
                byte b = Blend(PaperColour.B, InkColour.B, coverage);

                int i = y * stride + x * 4;
                pixels[i + 0] = b;
                pixels[i + 1] = g;
                pixels[i + 2] = r;
                pixels[i + 3] = 255;
            }
        }

        bitmap.WritePixels(new Int32Rect(0, 0, w, h), pixels, stride, 0);
    }

    // -----------------------------------------------------------------------
    // Core geometry
    // -----------------------------------------------------------------------

    /// <summary>
    /// Returns an ink coverage value in [0, 1] for the pixel at (px, py).
    /// 1 = fully on a line; 0 = fully off. The transition is smoothed over
    /// a 1-pixel band to eliminate angle-dependent aliasing.
    /// </summary>
    private static double LineCoverage(
       double px,
       double py,
       double cosA,
       double sinA,
       double spacing,
       double halfLine,
       RenderContext context,
       double waveAmplitude,
       double waveFrequency)
    {
        //
        // Coordinate system:
        //
        // projection = distance perpendicular to hatch line
        // along      = distance travelling along hatch line
        //

        double projection =
            px * cosA +
            py * sinA;

        double along =
            -px * sinA +
             py * cosA;


        // ------------------------------------------------------------
        // Wavy line deformation
        // ------------------------------------------------------------

        if (waveAmplitude > 0 && context.Noise != null)
        {
            // Smooth wandering of the stroke.
            // This moves the line centre sideways.
            double wave =
                context.Noise.Fractal(
                    along * waveFrequency,
                    0,
                    4)
                * waveAmplitude;

            projection += wave;
        }


        // ------------------------------------------------------------
        // Find distance to nearest hatch line
        // ------------------------------------------------------------

        double t = projection % spacing;

        if (t < 0)
            t += spacing;

        double gap =
            Math.Min(t, spacing - t);


        // ------------------------------------------------------------
        // Anti-aliased edge
        // ------------------------------------------------------------

        return Smoothstep(
            halfLine + 0.5,
            halfLine - 0.5,
            gap);
    }

    /// <summary>
    /// GLSL-style smoothstep: returns 0 when x >= edge0, 1 when x <= edge1,
    /// and a smooth cubic blend in between.
    /// </summary>
    private static double Smoothstep(double edge0, double edge1, double x)
    {
        double t = Math.Clamp((x - edge0) / (edge1 - edge0), 0.0, 1.0);
        return t * t * (3.0 - 2.0 * t);
    }

    private static byte Blend(byte a, byte b, double t)
        => (byte)Math.Clamp((int)(a + (b - a) * t), 0, 255);
}