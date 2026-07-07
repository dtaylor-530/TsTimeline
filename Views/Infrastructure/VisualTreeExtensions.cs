using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Views
{
    public static class VisualTreeExtensions
    {
        public static T? FindParent<T>(this DependencyObject child) where T : DependencyObject =>
            VisualTreeHelper.GetParent(child) switch
            {
                null => null,
                T parent => parent,
                { } parent => FindParent<T>(parent)
            };

        public static T? FindChild<T>(this FrameworkElement root, Func<FrameworkElement, bool>? compare = null) where T : FrameworkElement
        {
            compare ??= x => true;

            var children = Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(root)).Select(x => VisualTreeHelper.GetChild(root, x)).OfType<FrameworkElement>().ToArray();

            foreach (var child in children)
            {
                if (child is T t && compare(child))
                    return t;

                t = child.FindChild<T>(compare);
                if (t != null)
                    return t;
            }
            return null;
        }
    }
}