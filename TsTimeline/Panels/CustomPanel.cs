using System;
using System.Windows;
using System.Windows.Controls;
using Renderers;

namespace TsTimeline
{
    public partial class CustomPanel : Canvas
    {
        public static readonly DependencyProperty ViewportXProperty =
            DependencyProperty.Register(nameof(ViewportX), typeof(Viewport), typeof(CustomPanel), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
        
        public static readonly DependencyProperty ViewportYProperty =
            DependencyProperty.Register(nameof(ViewportY), typeof(Viewport), typeof(CustomPanel), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

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
                _ => base.MeasureOverride(constraint),
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
                ViewportX?.ViewportLength * ViewportX?.Scale * ViewportX?.Zoom ?? size.Width,
                ViewportY?.ViewportLength * ViewportY?.Scale * ViewportY?.Zoom ?? size.Height); ;
        }
    }
}
