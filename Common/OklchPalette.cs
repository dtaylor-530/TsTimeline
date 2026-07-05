using System.Drawing;

namespace SandBox
{
    public static class OklchPalette
    {
        private const double Deg2Rad = Math.PI / 180.0;

        private static readonly Random _random = new();

        /// <summary>
        /// Generates a palette of visually distinct colors in OKLCH space, optimized for contrast with white backgrounds
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<Color> Generate(int count)
        {
            var candidates = GenerateCandidates(4000);

            var palette = new List<Candidate>();

            // Seed: best contrast + chroma
            var seed = candidates
                .OrderByDescending(c => c.Oklch.L)
                .ThenByDescending(c => c.Oklch.C)
                .First();

            palette.Add(seed);
            candidates.Remove(seed);

            while (palette.Count < count)
            {
                Candidate best = null!;
                double bestScore = double.MinValue;

                foreach (var c in candidates)
                {
                    double minDist = double.MaxValue;

                    foreach (var p in palette)
                    {
                        double d = Distance(c, p);
                        if (d < minDist)
                            minDist = d;
                    }

                    // pure farthest-point selection
                    if (minDist > bestScore)
                    {
                        bestScore = minDist;
                        best = c;
                    }
                }

                palette.Add(best);
                candidates.Remove(best);
            }

            return palette.Select(c => c.Color).ToList();
        }

        /// <summary>
        /// Generates a list of random candidate colors in OKLCH space, filtered for contrast with white
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private static List<Candidate> GenerateCandidates(int count)
        {
            var list = new List<Candidate>(count);

            for (int i = 0; i < count; i++)
            {
                double h = _random.NextDouble() * 360;

                // Key design choice:
                // keep lightness mid-range for white backgrounds
                double L = 0.35 + _random.NextDouble() * 0.35; // 0.35–0.70 (OKLCH scale)
                double C = 0.08 + _random.NextDouble() * 0.18; // safe chroma

                var oklch = new Oklch(L, C, h);

                var rgb = OklchToRgb(oklch);

                if (!rgb.HasValue)
                    continue;

                var color = rgb.Value;

                if (ContrastWithWhite(color) < 3.0)
                    continue;

                list.Add(new Candidate
                {
                    Oklch = oklch,
                    Color = color
                });
            }

            return list;
        }


        /// <summary>
        /// Perceptual distance in OKLCH space (squared)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static double Distance(Candidate a, Candidate b)
        {
            // perceptual distance in OKLCH space
            double dl = a.Oklch.L - b.Oklch.L;
            double dc = a.Oklch.C - b.Oklch.C;

            double dh = AngleDistance(a.Oklch.H, b.Oklch.H);

            return
                dl * dl +
                dc * dc +
                0.5 * dh * dh;
        }

        /// <summary>
        /// Returns the shortest distance between two angles in degrees (0–360).
        /// </summary>
        /// <param name="h1"></param>
        /// <param name="h2"></param>
        /// <returns></returns>
        private static double AngleDistance(double h1, double h2)
        {
            double d = Math.Abs(h1 - h2) % 360;
            return d > 180 ? 360 - d : d;
        }

        /// <summary>
        /// Returns a new color with the specified opacity (alpha) value.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="opacity"></param>
        /// <returns></returns>
        public static Color WithOpacity(this Color original, double opacity)
        {
            return Color.FromArgb(
    (byte)(opacity * 255),              // alpha (0–255) → 204 ≈ 80%
    original.R,
    original.G,
    original.B);
        }

        /// <summary>
        /// OKLCH → RGB conversion (approximate)
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static Color? OklchToRgb(Oklch c)
        {
            // Convert OKLCH → OKLab
            double h = c.H * Deg2Rad;

            double a = c.C * Math.Cos(h);
            double b = c.C * Math.Sin(h);

            // OKLab → linear RGB (approx conversion)
            double L = c.L;

            double l_ = L + 0.3963377774 * a + 0.2158037573 * b;
            double m_ = L - 0.1055613458 * a - 0.0638541728 * b;
            double s_ = L - 0.0894841775 * a - 1.2914855480 * b;

            l_ = l_ * l_ * l_;
            m_ = m_ * m_ * m_;
            s_ = s_ * s_ * s_;

            double r = +4.0767416621 * l_ - 3.3077115913 * m_ + 0.2309699292 * s_;
            double g = -1.2684380046 * l_ + 2.6097574011 * m_ - 0.3413193965 * s_;
            double bl = -0.0041960863 * l_ - 0.7034186147 * m_ + 1.7076147010 * s_;

            if (r < 0 || r > 1 || g < 0 || g > 1 || bl < 0 || bl > 1)
                return null;

            return Color.FromArgb(
                (byte)255,
                (byte)(r * 255),
                (byte)(g * 255),
                (byte)(bl * 255));
        }

        /// <summary>
        /// Calculates the contrast ratio of a color against white
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static double ContrastWithWhite(Color c)
        {
            double l = Luminance(c);
            return (1.05) / (l + 0.05);
        }

        /// <summary>
        /// Calculates the relative luminance of a color
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static double Luminance(Color c)
        {
            double R = Linear(c.R / 255.0);
            double G = Linear(c.G / 255.0);
            double B = Linear(c.B / 255.0);

            return 0.2126 * R + 0.7152 * G + 0.0722 * B;
        }

        /// <summary>
        /// Converts a color channel value to linear space 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private static double Linear(double v)
            => v <= 0.04045 ? v / 12.92 : Math.Pow((v + 0.055) / 1.055, 2.4);

        private struct Oklch
        {
            public double L;
            public double C;
            public double H;

            public Oklch(double l, double c, double h)
            {
                L = l;
                C = c;
                H = h;
            }
        }

        private class Candidate
        {
            public Oklch Oklch;
            public Color Color;
        }
    }
}