using System.Windows;

namespace TsTimeline
{
    public sealed class LabelLayer : IAxisLayer
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
}