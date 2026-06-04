using System.Windows;
using System.Windows.Media;

namespace TsTimeline
{
    public sealed class TickLayer : IAxisLayer
    {
        private readonly Pen _pen;

        public TickLayer()
        {
            _pen = new Pen(
                Brushes.Gray,
                1);

            _pen.Freeze();
        }

        public void Render(AxisRenderContext context)
        {
            var dc = context.DrawingContext;

            foreach (var tick in context.Model.Ticks)
            {
                if (tick.ScreenPosition < -100)
                    continue;

                if (tick.ScreenPosition > context.Bounds.Width + 100)
                    continue;

                double height =
                    tick.Level switch
                    {
                        TickLevel.Major => 10,
                        TickLevel.Medium => 7,
                        _ => 4
                    };

                dc.DrawLine(
                    _pen,
                    new Point(tick.ScreenPosition, 0),
                    new Point(tick.ScreenPosition, height));
            }
        }
    }
}