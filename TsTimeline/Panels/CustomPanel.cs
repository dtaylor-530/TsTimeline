using System;
using System.Windows;
using System.Windows.Controls;
using Renderers;

namespace TsTimeline
{
    public partial class CustomPanel : Canvas
    {
        public static readonly DependencyProperty ViewportProperty =
            DependencyProperty.Register(nameof(Viewport), typeof(Viewport), typeof(CustomPanel), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty PanelTypeProperty =
            DependencyProperty.Register(nameof(PanelType), typeof(PanelType), typeof(CustomPanel), 
                new FrameworkPropertyMetadata(PanelType.None, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public Viewport Viewport
        {
            get { return (Viewport)GetValue(ViewportProperty); }
            set { SetValue(ViewportProperty, value); }
        }

        public PanelType PanelType
        {
            get { return (PanelType)GetValue(PanelTypeProperty); }
            set { SetValue(PanelTypeProperty, value); }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var size = base.MeasureOverride(constraint);
            switch (PanelType)
            {
                case PanelType.Canvas:
                    return Custom_MeasureOverride(constraint);
                case PanelType.DirectionalStackPanel:
                    return DirectionalStackPanel_MeasureOverride(constraint);
                case PanelType.ScrollAwareStackPanel:
                    return ScrollAwareStackPanel_MeasureOverride(constraint);
                default:
                    return base.MeasureOverride(constraint);
            }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            switch (PanelType)
            {
                case PanelType.Canvas:
                    return base.ArrangeOverride(arrangeSize);
                case PanelType.DirectionalStackPanel:
                    return DirectionalStackPanel_ArrangeOverride(arrangeSize);
                case PanelType.ScrollAwareStackPanel:
                    return DirectionalStackPanel_ArrangeOverride(arrangeSize);
                default:
                    return base.ArrangeOverride(arrangeSize);
            }
        }

        protected Size Custom_MeasureOverride(Size availableSize)
        {
            var size = base.MeasureOverride(availableSize);

            return new Size(Viewport?.ViewportWidth * Viewport?.ScaleX * Viewport?.ZoomX ?? size.Width, size.Height); ;
        }
    }
}
