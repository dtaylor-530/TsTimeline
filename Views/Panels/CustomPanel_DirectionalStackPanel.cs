namespace Views
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

            if (Direction == Direction.Up || Direction == Direction.Down)
            {
                foreach (UIElement child in InternalChildren)
                {
                    child.Measure(new Size(availableSize.Width, double.PositiveInfinity));

                    width = Math.Max(width, child.DesiredSize.Width);
                    height += child.DesiredSize.Height;
                }

                return new Size(0/*ViewportX?.Length * ViewportX?.Zoom ?? width*/, height);
            }
            else if (Direction == Direction.Right || Direction == Direction.Left)
            {
                foreach (UIElement child in InternalChildren)
                {
                    child.Measure(new Size(double.PositiveInfinity, availableSize.Height));

                    height = Math.Max(height, child.DesiredSize.Height);  
                    width += child.DesiredSize.Width;
                }

                //return new Size(width, ViewportY?.Length * ViewportY?.Zoom ?? height);
                return new Size(500/*ViewportX?.Length * ViewportX?.Zoom ?? width*/, height);
            }
            throw new NotImplementedException();
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
                        child.DesiredSize.Width,
                        child.DesiredSize.Height));

                    y += child.DesiredSize.Height;// child.DesiredSize.Height;
                }
            }
            else if(Direction == Direction.Up) 
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
            else if (Direction == Direction.Right)
            {
                double x = 0;

                foreach (UIElement child in InternalChildren)
                {
                    child.Arrange(new Rect(
                        x,
                        0,
                        finalSize.Width,
                        child.DesiredSize.Height));

                    x += child.DesiredSize.Width;
                }
            }
            else // Left
            {
                double x = finalSize.Height;

                foreach (UIElement child in InternalChildren)
                {
                    x -= child.DesiredSize.Height;

                    child.Arrange(new Rect(
                        x,
                        0,
                        finalSize.Width,
                        child.DesiredSize.Height));
                }
            }

            return finalSize;
        }
    }
}
