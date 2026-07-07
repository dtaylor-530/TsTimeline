namespace Views
{
    public class FlexPanel : Panel
    {

        public static readonly DependencyProperty AvailableHeightProperty =
    DependencyProperty.RegisterAttached(
        "AvailableHeight",
        typeof(double),
        typeof(FlexPanel),
        new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static void SetAvailableHeight(UIElement element, double value)
            => element.SetValue(AvailableHeightProperty, value);

        public static double GetAvailableHeight(UIElement element)
            => (double)element.GetValue(AvailableHeightProperty);

        public static readonly DependencyProperty AvailableWidthProperty =
    DependencyProperty.RegisterAttached(
        "AvailableWidth",
        typeof(double),
        typeof(FlexPanel),
        new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static void SetAvailableWidth(UIElement element, double value)
            => element.SetValue(AvailableWidthProperty, value);

        public static double GetAvailableWidth(UIElement element)
            => (double)element.GetValue(AvailableWidthProperty);

        public Direction Direction
        {
            get => (Direction)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(
                nameof(Direction),
                typeof(Direction),
                typeof(FlexPanel),
                new FrameworkPropertyMetadata(
                    Direction.Down,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty GridLengthProperty =
            DependencyProperty.RegisterAttached(
                "GridLength",
                typeof(GridLength),
                typeof(FlexPanel),
                new FrameworkPropertyMetadata(
                    GridLength.Auto,
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure));
        private ScrollViewer? scrollViewer;
        private TreeView? treeView;

        public static void SetGridLength(UIElement element, GridLength value)
            => element.SetValue(GridLengthProperty, value);

        public static GridLength GetGridLength(UIElement element)
            => (GridLength)element.GetValue(GridLengthProperty);

        protected override Size MeasureOverride(Size availableSize)
        {
            bool horizontal =
                Direction == Direction.Right ||
                Direction == Direction.Left;

            double fixedSpace = 0;
            double maxCross = 0;
            double starWeight = 0;

            double height = availableSize.Height, width = availableSize.Width;
         
            if (scrollViewer == null)
            {
                scrollViewer = VisualTreeExtensions.FindParent<ScrollViewer>(this);
                treeView = VisualTreeExtensions.FindParent<TreeView>(this);
                scrollViewer.SizeChanged += (s, e) =>
                {
                    InvalidateMeasure();
                };
                treeView.Loaded += (s, e) =>
                {
                    InvalidateMeasure();

                };
            }

       
            var treeViewItem = VisualTreeExtensions.FindParent<TreeViewItem>(this);
            var treeViewItem2 = VisualTreeExtensions.FindParent<TreeViewItem>(treeViewItem);

     

            //scrollViewer.Loaded += (s,e)=> this.InvalidateMeasure();
            //scrollViewer.ScrollChanged += (s, e) => this.InvalidateMeasure();
            if (double.IsInfinity(height))
            {
                if (scrollViewer != null)
                {
                    height = scrollViewer.ViewportHeight;
                }
            }

            if (double.IsInfinity(width))
            {
                if (scrollViewer != null)
                {
                    width = scrollViewer.ViewportWidth;
                }
            }

            var sum = new Size();
            foreach (UIElement child in InternalChildren)
            {
                child.Measure(
                                horizontal
                                    ? new Size(double.PositiveInfinity, availableSize.Height)
                                    : new Size(availableSize.Width, double.PositiveInfinity));
                sum = horizontal ?
                    new Size(sum.Width + child.DesiredSize.Width, Math.Max(sum.Height, child.DesiredSize.Height)) :
                    new Size(Math.Max(sum.Width, child.DesiredSize.Width), sum.Height + child.DesiredSize.Height);
            }

            //if (treeViewItem2 == null)
            //{
            //    return new Size(SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
            //}
            //availableSize = new Size(width, height);

            if (treeView.ActualHeight != 0)
            {
                foreach (UIElement child in InternalChildren)
                {
                    child.Measure(new Size(treeView.ActualWidth - 32, treeView.ActualHeight ));
                                 
        
                }

                return new Size(treeView.ActualWidth - 32, treeView.ActualHeight );
            }

            return sum;

            //
            // First pass: measure Auto and Pixel children.
            //
            foreach (UIElement child in InternalChildren)
            {
                if (child == null)
                    continue;

                GridLength dim = GetGridLength(child);

                switch (dim.GridUnitType)
                {
                    case GridUnitType.Auto:
                        {
                            child.Measure(
                                horizontal
                                    ? new Size(double.PositiveInfinity, availableSize.Height)
                                    : new Size(availableSize.Width, double.PositiveInfinity));

                            fixedSpace += horizontal
                                ? child.DesiredSize.Width
                                : child.DesiredSize.Height;

                            maxCross = Math.Max(
                                maxCross,
                                horizontal
                                    ? child.DesiredSize.Height
                                    : child.DesiredSize.Width);

                            break;
                        }

                    case GridUnitType.Pixel:
                        {
                            double length = dim.Value;

                            child.Measure(
                                horizontal
                                    ? new Size(length, availableSize.Height)
                                    : new Size(availableSize.Width, length));

                            fixedSpace += length;

                            maxCross = Math.Max(
                                maxCross,
                                horizontal
                                    ? child.DesiredSize.Height
                                    : child.DesiredSize.Width);

                            break;
                        }

                    case GridUnitType.Star:
                        starWeight += dim.Value;
                        break;
                }
            }

            double availableMain =
                horizontal ? availableSize.Width : availableSize.Height;

            if (double.IsInfinity(availableMain))
            {
                //
                // When unconstrained, measure stars as autos.
                //
                foreach (UIElement child in InternalChildren)
                {
                    if (child == null)
                        continue;

                    var dim = GetGridLength(child);

                    if (dim.GridUnitType != GridUnitType.Star)
                        continue;

                    child.Measure(
                        horizontal
                            ? new Size(double.PositiveInfinity, availableSize.Height)
                            : new Size(availableSize.Width, double.PositiveInfinity));

                    fixedSpace += horizontal
                        ? child.DesiredSize.Width
                        : child.DesiredSize.Height;

                    maxCross = Math.Max(
                        maxCross,
                        horizontal
                            ? child.DesiredSize.Height
                            : child.DesiredSize.Width);
                }

                return horizontal
                    ? new Size(fixedSpace, maxCross)
                    : new Size(maxCross, fixedSpace);
            }

            double remaining = Math.Max(0, availableMain - fixedSpace);

            //
            // Measure stars.
            //
            foreach (UIElement child in InternalChildren)
            {
                if (child == null)
                    continue;

                var dim = GetGridLength(child);

                if (dim.GridUnitType != GridUnitType.Star)
                    continue;

                double length =
                    starWeight <= 0
                        ? 0
                        : remaining * (dim.Value / starWeight);

                child.Measure(
                    horizontal
                        ? new Size(length, availableSize.Height)
                        : new Size(availableSize.Width, length));

                maxCross = Math.Max(
                    maxCross,
                    horizontal
                        ? child.DesiredSize.Height
                        : child.DesiredSize.Width);
            }

            return horizontal
                ? new Size(
                    double.IsInfinity(availableSize.Width)
                        ? fixedSpace
                        : availableSize.Width,
                    maxCross)
                : new Size(
                    maxCross,
                    double.IsInfinity(availableSize.Height)
                        ? fixedSpace
                        : availableSize.Height);
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        //protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        //{
        //    base.OnRenderSizeChanged(sizeInfo);

        //    InvalidateMeasure();
        //}

        protected override Size ArrangeOverride(Size finalSize)
        {
            bool horizontal =
           Direction == Direction.Right ||
           Direction == Direction.Left;

            double fixedSpace = 0;
            double totalStarWeight = 0;

            foreach (UIElement child in InternalChildren)
            {
                if (child == null)
                    continue;

                var dim = GetGridLength(child);

                if (dim.GridUnitType == GridUnitType.Pixel)
                    fixedSpace += dim.Value;
                else if (dim.GridUnitType == GridUnitType.Auto)
                    fixedSpace += horizontal
                        ? child.DesiredSize.Width
                        : child.DesiredSize.Height;
                else
                    totalStarWeight += dim.Value;
            }

            double availableMain =
                horizontal ? finalSize.Width : finalSize.Height;

            double remaining = Math.Max(0, availableMain - fixedSpace);

            //double offset =
            //  Direction == Direction.Left
            //      ? finalSize.Width
            //      : Direction == Direction.Down
            //          ? finalSize.Height
            //          : 0;
            double offset = 0;

            foreach (UIElement child in InternalChildren)
            {
                if (child == null)
                    continue;

                var dim = GetGridLength(child);

                double length;

                switch (dim.GridUnitType)
                {
                    case GridUnitType.Pixel:
                        length = dim.Value;
                        break;

                    case GridUnitType.Auto:
                        length = horizontal
                            ? child.DesiredSize.Width
                            : child.DesiredSize.Height;
                        break;

                    default:
                        length = totalStarWeight == 0
                            ? 0
                            : remaining * (dim.Value / totalStarWeight);
                        break;
                }

                Rect rect;

                switch (Direction)
                {
                    case Direction.Right:
                        rect = new Rect(offset, 0, length, finalSize.Height);
                        offset += length;
                        break;

                    case Direction.Left:
                        offset -= length;
                        rect = new Rect(offset, 0, length, finalSize.Height);
                        break;

                    case Direction.Down:
                        rect = new Rect(0, offset, finalSize.Width, length);
                        offset += length;
                        break;

                    case Direction.Up:
                        offset -= length;
                        rect = new Rect(0, offset, finalSize.Width, length);
                        break;

                    default:
                        throw new InvalidOperationException();
                }
                SetAvailableHeight(child, rect.Height);
                child.Arrange(rect);
            }

            return finalSize;
        }
    }
}

