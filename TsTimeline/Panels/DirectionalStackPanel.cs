using System;
using System.Windows;

namespace TsTimeline
{
    /// <summary>
    /// DirectionalStackPanel
    /// </summary>
    public partial class CustomPanel
    {
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(
                nameof(Direction),
                typeof(Direction),
                typeof(CustomPanel),
                new FrameworkPropertyMetadata(
                    Direction.Down,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsArrange));

        public Direction Direction
        {
            get => (Direction)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        protected Size DirectionalStackPanel_MeasureOverride(Size availableSize)
        {
            double width = 0;
            double height = 0;

            foreach (UIElement child in InternalChildren)
            {
                child.Measure(new Size(availableSize.Width, double.PositiveInfinity));

                width = Math.Max(width, child.DesiredSize.Width);
                height += child.DesiredSize.Height;
            }

            return new Size(ViewportX?.ViewportLength * ViewportX?.Zoom ?? width, height);
        }

        protected Size DirectionalStackPanel_ArrangeOverride(Size finalSize)
        {
            if (Direction == Direction.Down)
            {
                double y = 0;

                foreach (UIElement child in InternalChildren)
                {
                    child.Arrange(new Rect(
                        0,
                        y,
                        finalSize.Width,
                        child.DesiredSize.Height));

                    y += child.DesiredSize.Height;
                }
            }
            else // Up
            {
                double y = finalSize.Height;

                foreach (UIElement child in InternalChildren)
                {
                    y -= child.DesiredSize.Height;

                    child.Arrange(new Rect(
                        0,
                        y,
                        finalSize.Width,
                        child.DesiredSize.Height));
                }
            }

            return finalSize;
        }
    }
}
