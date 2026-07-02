using System;
using System.Windows;

namespace Renderers
{
    public sealed class XTopLabelLayer : IAxisLayer
    {
        public IFormatter Formatter { get; set; }

        public double Offset { get; set; } = - 2;
        public void Render(AxisRenderContext context)
        {     
            foreach (var tick in context.Ticks)
            {
                if (tick.Level != TickLevel.Major)
                    continue;
     
                string text =
                    Formatter.Format(
                        tick.Value);

                var formatted =
                    context.LabelCache.Get(text);

                context.DrawingContext.DrawText(
                    formatted,
                    new Point(
                        tick.ScreenPosition + Offset,
                        0));
            }
        }
    }

    public sealed class XBottomLabelLayer : IAxisLayer
    {
        public IFormatter Formatter { get; set; }

        public double Offset { get; set; } = 2;
        public void Render(AxisRenderContext context)
        {
            foreach (var tick in context.Ticks)
            {
                if (tick.Level != TickLevel.Major)
                    continue;
                     
                string text =
                    Formatter.Format(
                        tick.Value);

                var formatted =
                    context.LabelCache.Get(text);

                context.DrawingContext.DrawText(
                    formatted,
                    new Point(
                        tick.ScreenPosition + Offset,
                        context.Bounds.Height));
            }
        }
    }

    public sealed class YLabelLayer : IAxisLayer
    {
        public IFormatter Formatter { get; set; }

        public double Offset { get; set; } = -30;

        public void Render(AxisRenderContext context)
        {
            foreach (var tick in context.Ticks)
            {
                if (tick.Level != TickLevel.Major)
                    continue;

                string text = Formatter.Format(tick.Value);

                var formatted = context.LabelCache.Get(text);

                context.DrawingContext.DrawText(formatted, new Point(Offset, tick.ScreenPosition ));
            }
        }
    }

}