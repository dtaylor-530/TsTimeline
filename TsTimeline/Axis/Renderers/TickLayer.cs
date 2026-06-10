using System.Windows;
using System.Windows.Media;

namespace TsTimeline
{
    public sealed class XTickLayer : IAxisLayer
    {
        private readonly Pen _pen;

        public XTickLayer()
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

    public sealed class XUpTickLayer : IAxisLayer
    {
        private readonly Pen _pen;

        public XUpTickLayer()
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
                    new Point(tick.ScreenPosition, context.Bounds.Height),
                    new Point(tick.ScreenPosition, context.Bounds.Height - height));
            }

        }
    }
    public sealed class YTickLayer : IAxisLayer
    {
        private readonly Pen _pen;

        public YTickLayer()
        {
            _pen = new Pen(Brushes.Gray, 1);
            _pen.Freeze();
        }

        public void Render(AxisRenderContext context)
        {
            var dc = context.DrawingContext;

            foreach (var tick in context.Model.Ticks)
            {
                if (tick.ScreenPosition < -100)
                    continue;

                if (tick.ScreenPosition > context.Bounds.Height + 100)
                    continue;

                double width =
                    tick.Level switch
                    {
                        TickLevel.Major => 10,
                        TickLevel.Medium => 7,
                        _ => 4
                    };

                dc.DrawLine(
                    _pen,
                    new Point(tick.ScreenPosition, width),
                    new Point(tick.ScreenPosition, 0));
            }
        }
    }
}