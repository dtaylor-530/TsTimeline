using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace TsTimeline
{
    public partial class CustomPanel : Canvas
    {
        public static readonly DependencyProperty ViewportXProperty =
            DependencyProperty.Register(nameof(ViewportX), typeof(Viewport), typeof(CustomPanel),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, change));

        private static void change(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomPanel panel && e.NewValue is Viewport viewport)
            {
                viewport.PropertyChanged += (s, e) =>
                {
                    if (
                    e.PropertyName == nameof(Viewport.Start) ||
                    e.PropertyName == nameof(Viewport.End) ||
                    e.PropertyName == nameof(Viewport.Zoom))
                        panel.InvalidateMeasure();
                };
            }
        }

        public static readonly DependencyProperty ViewportYProperty =
            DependencyProperty.Register(nameof(ViewportY), typeof(Viewport), typeof(CustomPanel),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, change));

        public static readonly DependencyProperty PanelTypeProperty =
            DependencyProperty.Register(nameof(PanelType), typeof(PanelType), typeof(CustomPanel),
                new FrameworkPropertyMetadata(PanelType.None, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public Viewport ViewportX
        {
            get { return (Viewport)GetValue(ViewportXProperty); }
            set { SetValue(ViewportXProperty, value); }
        }

        public Viewport ViewportY
        {
            get { return (Viewport)GetValue(ViewportYProperty); }
            set { SetValue(ViewportYProperty, value); }
        }

        public PanelType PanelType
        {
            get { return (PanelType)GetValue(PanelTypeProperty); }
            set { SetValue(PanelTypeProperty, value); }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            //var size = base.MeasureOverride(constraint);
            return PanelType switch
            {
                PanelType.Canvas => Canvas_MeasureOverride(constraint),
                PanelType.Map => Map_MeasureOverride(constraint),
                PanelType.DirectionalStackPanel => DirectionalStackPanel_MeasureOverride(constraint),
                PanelType.AutoGrid => AutoGrid_MeasureOverride(constraint),
                _ => base.MeasureOverride(constraint),
            };
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            return PanelType switch
            {
                PanelType.Canvas => base.ArrangeOverride(arrangeSize),
                PanelType.DirectionalStackPanel => DirectionalStackPanel_ArrangeOverride(arrangeSize),
                PanelType.AutoGrid => AutoGrid_ArrangeOverride(arrangeSize),
                _ => base.ArrangeOverride(arrangeSize),
            };
        }


        protected Size Canvas_MeasureOverride(Size availableSize)
        {
            var treeViewItem = VisualTreeExtensions.FindParent<TreeViewItem>(this);
            var treeViewItem2 = VisualTreeExtensions.FindParent<TreeViewItem>(treeViewItem);
            var scrollViewer = VisualTreeExtensions.FindParent<ScrollViewer>(this);
            var availableHeight = FlexPanel.GetAvailableHeight(treeViewItem);
            var availableWidth = FlexPanel.GetAvailableWidth(treeViewItem);
            base.MeasureOverride(availableSize);
            double maxLeft = 0;
            double maxTop = 0;
            foreach (UIElement child in InternalChildren)
            {
                child.Measure(new Size(availableSize.Width, availableSize.Height));
                var size = child.DesiredSize;
                //var left = Canvas.GetLeft(child);
                //var top = Canvas.GetTop(child);

                //maxLeft = Math.Max(maxLeft, left + child.DesiredSize.Width);
                //maxTop = Math.Max(maxTop, top + child.DesiredSize.Height);
                if (child is ClipBase { X: { } x, Y: { } y, DataContext:Notification{ Key: "Point" } })
                {
                    maxLeft = Math.Max(maxLeft, (x + child.DesiredSize.Width) * ViewportX.Zoom);
                    maxTop = Math.Max(maxTop, (y + child.DesiredSize.Height) * ViewportY.Zoom);
                }
            }
            return new Size(maxLeft, maxTop);
            //if (double.IsInfinity(availableSize.Height))
            //    return new Size(maxLeft, maxTop);
            //return new Size(treeViewItem.ActualWidth, treeViewItem.ActualHeight);
        }

        protected Size Map_MeasureOverride(Size availableSize)
        {
            var treeViewItem = VisualTreeExtensions.FindParent<TreeViewItem>(this);
            var treeViewItem2 = VisualTreeExtensions.FindParent<TreeViewItem>(treeViewItem);
            var scrollViewer = VisualTreeExtensions.FindParent<ScrollViewer>(this);
            var availableHeight = FlexPanel.GetAvailableHeight(treeViewItem);
            var availableWidth = FlexPanel.GetAvailableWidth(treeViewItem);
            base.MeasureOverride(availableSize);
            double maxWidth = 0;
            double maxHeight = 0;

            foreach (FrameworkElement child in InternalChildren)
            {
                if (child.DataContext is ISize size)
                {
                    maxWidth = Math.Max(maxWidth, size.Left.Value + size.Width.Value);
                    maxHeight = Math.Max(maxWidth, size.Top.Value + size.Height.Value);
                }
                child.Measure(new Size(availableSize.Width, availableSize.Height));
                var _ = child.DesiredSize;
            }
            //if (double.IsInfinity(availableSize.Height))
            //    return new Size(0, 0);
            return new Size(maxWidth * ViewportX.Zoom, maxHeight * ViewportY.Zoom);
        }
    }
}
