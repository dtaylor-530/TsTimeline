using System.Collections.Generic;
using System.Collections.Specialized;

namespace Renderers
{
    public sealed class CombinationLayer : IAxisLayer, INotifyCollectionChanged
    {
        private readonly List<IAxisLayer> _layers;

        public CombinationLayer(params IAxisLayer[] layers)
        {
            _layers = new List<IAxisLayer>(layers);
        }

        public IEnumerable<IAxisLayer> Layers => _layers;

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public void AddLayer(IAxisLayer layer)
        {
            if (!_layers.Contains(layer))
            {
                _layers.Add(layer);

                CollectionChanged?.Invoke(
                    this,
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Add,
                        layer,
                        _layers.Count - 1));
            }
        }

        public void RemoveLayer(IAxisLayer layer)
        {
            int index = _layers.IndexOf(layer);

            if (index >= 0)
            {
                _layers.RemoveAt(index);

                CollectionChanged?.Invoke(
                    this,
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Remove,
                        layer,
                        index));
            }
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