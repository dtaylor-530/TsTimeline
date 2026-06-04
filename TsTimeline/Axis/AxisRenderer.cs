using System.Collections.Generic;

namespace TsTimeline
{
    public sealed class AxisRenderer
    {
        private readonly List<IAxisLayer> _layers =
            new();

        public AxisRenderer()
        {
        }

        public void AddLayer(
            IAxisLayer layer)
        {
            _layers.Add(layer);
        }

        public void Render(
            AxisRenderContext context)
        {
            foreach (var layer in _layers)
            {
                layer.Render(context);
            }
        }
    }
}