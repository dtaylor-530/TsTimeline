using System;
using System.Windows;

namespace Renderers
{
    public sealed class XLabelLayer : IAxisLayer
    {
        public void Render(AxisRenderContext context)
        {
            var dc = context.DrawingContext;

            foreach (var tick in context.Model.Ticks)
            {
                if (tick.Level != TickLevel.Major)
                    continue;

                if (tick.ScreenPosition < -100)
                    continue;

                if (tick.ScreenPosition > context.Bounds.Width + 100)
                    continue;
                string text =
                    context.LabelFormatter.Format(
                        tick.Value);

                var formatted =
                    context.LabelCache.Get(text);

                dc.DrawText(
                    formatted,
                    new Point(
                        tick.ScreenPosition + 2,
                        0));
            }
        }
    }
    public sealed class XUpLabelLayer : IAxisLayer
    {
        public void Render(AxisRenderContext context)
        {
            var dc = context.DrawingContext;

            foreach (var tick in context.Model.Ticks)
            {
                if (tick.Level != TickLevel.Major)
                    continue;

                if (tick.ScreenPosition < -100)
                    continue;

                if (tick.ScreenPosition > context.Bounds.Width + 100)
                    continue;
                string text =
                    context.LabelFormatter.Format(
                        tick.Value);

                var formatted =
                    context.LabelCache.Get(text);

                dc.DrawText(
                    formatted,
                    new Point(
                        tick.ScreenPosition + 2,
                        context.Bounds.Height));
            }
        }
    }

    public sealed class YLabelLayer : IAxisLayer
    {
        public void Render(AxisRenderContext context)
        {
            var dc = context.DrawingContext;

            foreach (var tick in context.Model.Ticks)
            {
                if (tick.Level != TickLevel.Major)
                    continue;

                if (tick.ScreenPosition < -100)
                    continue;

                if (tick.ScreenPosition > context.Bounds.Height + 100)
                    continue;

                string text = context.LabelFormatter.Format(tick.Value);

                var formatted = context.LabelCache.Get(text);

                dc.DrawText(formatted, new Point(0, tick.ScreenPosition + 2));
            }
        }
    }

}