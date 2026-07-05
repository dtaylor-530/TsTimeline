
namespace Common
{
    internal class CustomCollection : IReadOnlyCollection<object>, INotifyCollectionChanged
    {
        private List<object> _list = new();

        public int Count => _list.Count;

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        internal virtual void Add(params object[] objects)
        {
            if (objects.Length == 0)
                return;

            int index = _list.Count;

            _list.AddRange(objects);

            CollectionChanged?.Invoke(
                this,
                objects.Length == 1
                    ? new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Add,
                        objects[0],
                        index)
                    : new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Add,
                        (IList)objects,
                        index));
        }

        internal void Remove(object notification)
        {
            int index = _list.IndexOf(notification);

            if (index >= 0)
            {
                _list.RemoveAt(index);

                CollectionChanged?.Invoke(
                    this,
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Remove,
                        notification,
                        index));
            }
        }

        internal void Clear()
        {
            _list.Clear();
            CollectionChanged?.Invoke(
                this,
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Reset));
        }

        public IEnumerator<object> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}