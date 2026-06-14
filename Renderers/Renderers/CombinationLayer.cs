using System.Collections.Generic;

namespace Renderers
{
    public sealed class CombinationLayer : IAxisLayer
    {
        private readonly List<IAxisLayer> _layers;

        public CombinationLayer(params IAxisLayer[] layers)
        {
            _layers = new List<IAxisLayer>(layers);
        }

        public IEnumerable<IAxisLayer> Layers => _layers;

        public void AddLayer(IAxisLayer layer)
        {
            _layers.Add(layer);
        }

        public void Render(AxisRenderContext context)
        {
            foreach (var layer in _layers)
            {
                layer.Render(context);
            }
        }
    }
}