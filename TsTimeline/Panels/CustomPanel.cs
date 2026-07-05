using System;
using System.Windows;
using System.Windows.Controls;

namespace TsTimeline
{
    public partial class CustomPanel : Canvas
    {
        public static readonly DependencyProperty IncludeInMeasureProperty =
            DependencyProperty.RegisterAttached(
                "IncludeInMeasure",
                typeof(bool),
                typeof(CustomPanel),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static void SetIncludeInMeasure(UIElement element, bool value)
            => element.SetValue(IncludeInMeasureProperty, value);

        public static bool GetIncludeInMeasure(UIElement element)
            => (bool)element.GetValue(IncludeInMeasureProperty);


        public static readonly DependencyProperty ScaleXProperty =
            DependencyProperty.Register(nameof(ScaleX), typeof(double), typeof(CustomPanel), new PropertyMetadata(0d));

        public static readonly DependencyProperty ScaleYProperty =
            DependencyProperty.Register(nameof(ScaleY), typeof(double), typeof(CustomPanel), new PropertyMetadata(0d));

        

        public double ScaleX
        {
            get { return (double)GetValue(ScaleXProperty); }
            set { SetValue(ScaleXProperty, value); }
        }

        public double ScaleY
        {
            get { return (double)GetValue(ScaleYProperty); }
            set { SetValue(ScaleYProperty, value); }
        }

        private static void change(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomPanel panel /*&& e.NewValue is Viewport viewport*/)
            {
                //viewport.PropertyChanged += (s, e) =>
                //{
                    //if (
                    //e.PropertyName == nameof(Viewport.Start) ||
                    //e.PropertyName == nameof(Viewport.End) ||
                    //e.PropertyName == nameof(Viewport.Zoom))
                        panel.InvalidateMeasure();
                //};
            }
        }

    
        public static readonly DependencyProperty PanelTypeProperty =
            DependencyProperty.Register(nameof(PanelType), typeof(PanelType), typeof(CustomPanel),
                new FrameworkPropertyMetadata(PanelType.None, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

    
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

                if (child is ClipBase { X: { } x, Y: { } y} && GetIncludeInMeasure(child))
                {
                    maxLeft = Math.Max(maxLeft, (x + child.DesiredSize.Width) * ScaleX);
                    maxTop = Math.Max(maxTop, (y + child.DesiredSize.Height) *  ScaleY);
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
            return new Size(maxWidth * ScaleX, maxHeight * ScaleY);
        }
    }
}
