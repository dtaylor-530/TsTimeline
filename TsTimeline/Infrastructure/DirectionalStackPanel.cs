using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TsTimeline
{

    public class DirectionalStackPanel : Panel
    {
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(
                nameof(Direction),
                typeof(Direction),
                typeof(DirectionalStackPanel),
                new FrameworkPropertyMetadata(
                    Direction.Down,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsArrange));

        public Direction Direction
        {
            get => (Direction)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double width = 0;
            double height = 0;

            foreach (UIElement child in InternalChildren)
            {
                child.Measure(new Size(availableSize.Width, double.PositiveInfinity));

                width = Math.Max(width, child.DesiredSize.Width);
                height += child.DesiredSize.Height;
            }

            return new Size(width, height);
        }

        protected override Size ArrangeOverride(Size finalSize)
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
