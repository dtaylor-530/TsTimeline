using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Renderers
{

    public class Notification : INotifyPropertyChanged
    {
        CustomCollection customCollection = [];
        private string? group;

        public string Name { get; set; }

        public string Key { get; set; }

        public string? Group { get => group ??= FindParent(n => n.Group != null)?.Group; set => group = value; }

        public Notification? Parent { get; set; }

        public IEnumerable<object> Children
        {
            get => customCollection;
            set => Add([.. value]);
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool SetProperty<T>(ref T property, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(property, value))
                return false;

            property = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion PropertyChanged

        public virtual void Add(params object[] objects)
        {
            foreach (var obj in objects)
            {
                if (obj is Notification n)
                {
                    if (this.group != null)
                        n.Group = this.group;
                    n.Parent = this;
                }
            }
            customCollection.Add(objects);
        }
        public virtual void Remove(object notification) => customCollection.Remove(notification);
        public virtual void Clear() => customCollection.Clear();


        public Notification? FindParent(Predicate<Notification> predicate, Notification? parent = null)
        {
            parent ??= this.Parent;
            while (parent != null)
            {
                if (predicate(parent))
                {
                    return parent;
                }
                parent = parent.Parent;

            }
            return null;
        }

        public T? FindParent<T>(string name) where T : Notification
        {
            return (T?)FindParent(a => a is T && a.Key == name);
        }
        public object? FindChild(Predicate<object> predicate)
        {
            foreach (var child in Children)
            {
                if (predicate(child))
                    return child;
                else
                    if (child is Notification notification && notification.FindChild(predicate) is { } _child)
                        return _child;
            }
            return null;
        }
        public IEnumerable? FindChildren(Predicate<object> predicate)
        {
            foreach (var child in Children)
            {
                if (predicate(child))
                    yield return child;
                else
                    if (child is Notification notification && notification.FindChild(predicate) is { } _child)
                        yield return _child;
            }            
        }

        public T? FindChild<T>(string key) where T : Notification
        {
            return (T?)FindChild(a => a is T t && t.Key == key);
        }
        public IEnumerable<T> FindChildren<T>(string key) where T : Notification
        {
            return FindChildren(a => a is T t && t.Key == key).Cast<T>();
        }
    }
}