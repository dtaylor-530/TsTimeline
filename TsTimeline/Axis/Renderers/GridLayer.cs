using System.Windows;
using System.Windows.Media;

namespace TsTimeline
{
    public sealed class XGridLayer : IAxisLayer
    {
        private readonly Pen _majorPen;
        private readonly Pen _minorPen;

        public XGridLayer()
        {
            _majorPen = new Pen(Brushes.LightGray, 1);
            _minorPen = new Pen(Brushes.Gainsboro, 0.5);
            _majorPen.Freeze();
            _minorPen.Freeze();
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

                var pen = tick.Level == TickLevel.Major ? _majorPen : _minorPen;

                dc.DrawLine(pen,
                    new Point(tick.ScreenPosition, context.TickMargin),
                    new Point(tick.ScreenPosition, context.Bounds.Height));
            }
        }
    }
    public sealed class XUpGridLayer : IAxisLayer
    {
        private readonly Pen _majorPen;
        private readonly Pen _minorPen;

        public XUpGridLayer()
        {
            _majorPen = new Pen(Brushes.LightGray, 1);
            _minorPen = new Pen(Brushes.Gainsboro, 0.5);
            _majorPen.Freeze();
            _minorPen.Freeze();
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

                var pen = tick.Level == TickLevel.Major ? _majorPen : _minorPen;

                dc.DrawLine(pen,
                    new Point(tick.ScreenPosition, context.TickMargin),
                    new Point(tick.ScreenPosition, context.Bounds.Height));
            }
        }
    }

    public sealed class YGridLayer : IAxisLayer
    {
        private readonly Pen _majorPen;
        private readonly Pen _minorPen;

        public YGridLayer()
        {
            _majorPen = new Pen(Brushes.LightGray, 1);
            _minorPen = new Pen(Brushes.Gainsboro, 0.5);
            _majorPen.Freeze();
            _minorPen.Freeze();
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

                var pen = tick.Level == TickLevel.Major ? _majorPen : _minorPen;

                dc.DrawLine(
                    pen,
                    new Point(context.TickMargin, tick.ScreenPosition),
                    new Point(context.Bounds.Width, tick.ScreenPosition));
            }
        }
    }
}


