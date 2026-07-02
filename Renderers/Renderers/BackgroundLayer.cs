using System;
using System.Windows;
using System.Windows.Media;

namespace Renderers
{
    /// <summary>
    /// Alternating background colour layer
    /// </summary>
    public sealed class YBackgroundLayer(
        Brush evenBrush,
        Brush oddBrush) : IAxisLayer
    {
        private readonly Brush _evenBrush = evenBrush;
        private readonly Brush _oddBrush = oddBrush;

        public void Render(AxisRenderContext context)
        {
            var dc = context.DrawingContext;
            var ticks = context.Ticks.GetEnumerator();
            ticks.MoveNext();
            AxisTick last = ticks.Current;
            int i = 0;
            while (ticks.MoveNext() && ticks.Current.ScreenPosition < context.Bounds.Height)
            {
                i++;
                var diff = ticks.Current.ScreenPosition - last.ScreenPosition;
                var brush =
                 i % 2 == 0
                     ? _evenBrush
                     : _oddBrush;

                dc.DrawRectangle(
                    brush,
                    null,
                    new Rect(
                        0,
                        last.ScreenPosition,
                        context.Bounds.Width,
                        diff));
                last = ticks.Current;
            }
        }
    }


    public sealed class OceanBackgroundLayer(
        ) : IAxisLayer
    {
        private readonly RenderContext _context = new();

        public void Render(AxisRenderContext context)
        {
            var ocean = new CrosshatchTextureLayer
            {
                Angle = 45,
                SecondAngle = null,
                Spacing = 12,
                LineWidth = 1.2,
                InkColour = Colors.White,
                PaperColour = Color.FromRgb(215, 240, 255),
                WaveAmplitude = 10,
                JitterAmount = 3,
                WaveFrequency =0.1,
            };

            ocean.Draw(context.DrawingContext, context.Bounds, _context);



        }
    }

    public class CountryTextureLayer
    {
        public static void Draw(DrawingContext drawingContext)
        {
            var ocean = new CrosshatchTextureLayer { Angle = 45, SecondAngle = 150, Spacing = 2, 
                LineWidth = 0.6, JitterAmount = 4.6, 
                WaveAmplitude = 2.85, WaveFrequency = 0.16, 
                InkColour = Color.FromArgb(144, 145, 126, 120 ), 
                PaperColour = Color.FromArgb(142, 231, 255, 238) };

            ocean.Draw(drawingContext, new Rect(0, 0, 500, 500), new RenderContext());

        }
    }
}
