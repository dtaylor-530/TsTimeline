using System;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Controls;
using Renderers;

namespace TsTimeline
{
    public partial class CustomPanel : Canvas
    {
        public static readonly DependencyProperty ViewportXProperty =
            DependencyProperty.Register(nameof(ViewportX), typeof(Viewport), typeof(CustomPanel), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, change));

        private static void change(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is CustomPanel panel && e.NewValue is Viewport viewport)
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
            var size = base.MeasureOverride(constraint);
            return PanelType switch
            {
                PanelType.Canvas => Custom_MeasureOverride(constraint),
                PanelType.DirectionalStackPanel => DirectionalStackPanel_MeasureOverride(constraint),
                PanelType.ScrollAwareStackPanel => ScrollAwareStackPanel_MeasureOverride(constraint),
                _ => Custom_MeasureOverride(constraint),
            };
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            return PanelType switch
            {
                PanelType.Canvas => base.ArrangeOverride(arrangeSize),
                PanelType.DirectionalStackPanel => DirectionalStackPanel_ArrangeOverride(arrangeSize),
                PanelType.ScrollAwareStackPanel => DirectionalStackPanel_ArrangeOverride(arrangeSize),
                _ => base.ArrangeOverride(arrangeSize),
            };
        }

        protected Size Custom_MeasureOverride(Size availableSize)
        {
            var size = base.MeasureOverride(availableSize);

            return new Size(
                (ViewportX?.End - ViewportX?.Start) * ViewportX?.Zoom ?? size.Width,
                (ViewportY?.End - ViewportY?.Start) * ViewportY?.Zoom ?? size.Height); ;
        }
    }
}
