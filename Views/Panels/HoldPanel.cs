namespace Views
{
    public class HoldPanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                child.Measure(availableSize);
            }
            return new Size(availableSize.Width, double.IsInfinity(availableSize.Height)? 0: availableSize.Height );
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            //var x = finalSize.Width - 10;
            //if (finalSize.Width <= 5)
            //    return finalSize;
            foreach (UIElement child in InternalChildren)
            {
                var column = Grid.GetColumn(child);
             
                if (column == 0)
                {
                    var sizeRect = new Rect(-2.5, 0, 5, finalSize.Height);
                    CustomPanel.SetArrangeRect(child, sizeRect);
                    child.Arrange(sizeRect);
                }
                else if (column == 1)
                {
                    var sizeRect = new Rect(0, 0, finalSize.Width, finalSize.Height);
                    CustomPanel.SetArrangeRect(child, sizeRect);
                    child.Arrange(sizeRect);
                }
                else if (column == 2)
                {
                    var sizeRect = new Rect(finalSize.Width -2.5, 0, 5, finalSize.Height);
                    CustomPanel.SetArrangeRect(child, sizeRect);
                    child.Arrange(sizeRect);
                }
                else
                    throw new Exception("SDF 4 y56546h");
            }
            return finalSize;
        }
    }
}
