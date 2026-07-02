using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TsTimeline
{
    public partial class CustomPanel
    {
        private TreeViewItem treeViewItem;

        public static readonly DependencyProperty ArrangeRectProperty =
            DependencyProperty.RegisterAttached(
                "ArrangeRect",
                typeof(Rect),
                typeof(CustomPanel),
                new FrameworkPropertyMetadata(
                    default(Rect),
                    FrameworkPropertyMetadataOptions.AffectsArrange));

        public static void SetArrangeRect(DependencyObject element, Rect value)
        {
            element.SetValue(ArrangeRectProperty, value);
        }

        public static Rect GetArrangeRect(DependencyObject element)
        {
            return (Rect)element.GetValue(ArrangeRectProperty);
        }

        protected Size AutoGrid_MeasureOverride(Size availableSize)
        {
            Size maxChild = new();
            if (treeViewItem == null)
            {
                treeViewItem = VisualTreeExtensions.FindParent<TreeViewItem>(this);
                treeViewItem.SizeChanged += (s, e) => InvalidateMeasure();
            }
            //var available = FlexPanel.GetAvailableHeight(treeViewItem);
            //var availableWidth = FlexPanel.GetAvailableWidth(treeViewItem);

            foreach (UIElement child in InternalChildren)
            {
                child.Measure(new Size(treeViewItem.ActualWidth, treeViewItem.ActualHeight));

                maxChild.Width = Math.Max(maxChild.Width, child.DesiredSize.Width);
                maxChild.Height = Math.Max(maxChild.Height, child.DesiredSize.Height);
            }

            if (InternalChildren.Count == 0)
                return new Size();

            double availableWidth = double.IsInfinity(treeViewItem.ActualWidth)
                ? maxChild.Width * InternalChildren.Count
                : availableSize.Width;

            int columns = Math.Max(1, (int)(treeViewItem.ActualWidth / maxChild.Width));
            int rows = (int)Math.Ceiling((double)InternalChildren.Count / columns);

            return new Size(
                Math.Min(columns * maxChild.Width, treeViewItem.ActualWidth),
                rows * maxChild.Height);
        }

        protected Size AutoGrid_ArrangeOverride(Size finalSize)
        {
            if (InternalChildren.Count == 0)
                return finalSize;

            Size maxChild = new();

            foreach (UIElement child in InternalChildren)
            {
                maxChild.Width = Math.Max(maxChild.Width, child.DesiredSize.Width);
                maxChild.Height = Math.Min(maxChild.Height, child.DesiredSize.Height);
            }

            int columns = Math.Max(1, (int)(finalSize.Width / maxChild.Width));

            for (int i = 0; i < InternalChildren.Count; i++)
            {
                int row = i / columns;
                int column = i % columns;
                var child = InternalChildren[i];
                var rect = new Rect(
                    column * child.DesiredSize.Width,
                    row * child.DesiredSize.Height,
                    child.DesiredSize.Width,
                    child.DesiredSize.Height);
                SetArrangeRect(child, rect);
                child.Arrange(rect);
            }

            return finalSize;
        }
    }
}
