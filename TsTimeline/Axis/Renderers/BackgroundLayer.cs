using System.Windows;
using System.Windows.Media;

namespace TsTimeline
{
    /// <summary>
    /// Background colour layer
    /// </summary>
    /// 
    public sealed class XBackgroundLayer : IAxisLayer
    {
        private readonly Brush _evenBrush;
        private readonly Brush _oddBrush;

        public XBackgroundLayer(
            Brush evenBrush,
            Brush oddBrush)
        {
            _evenBrush = evenBrush;
            _oddBrush = oddBrush;
        }

        public void Render(AxisRenderContext context)
        {
            var dc = context.DrawingContext;

            double rowHeight = context.TrackHeight;

            for (int i = 0; i < context.TrackCount; i++)
            {
                var brush =
                    i % 2 == 0
                        ? _evenBrush
                        : _oddBrush;

                dc.DrawRectangle(
                    brush,
                    null,
                    new Rect(
                        0,
                        i * rowHeight,
                        context.Bounds.Width,
                        rowHeight));
            }

        }
    }
}
