using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TsTimeline
{
    public class ExpandPanel : Panel
    {

        protected override Size MeasureOverride(Size availableSize)
        {
            var treeViewItem = VisualTreeExtensions.FindParent<TreeViewItem>(this);
            var available = FlexPanel.GetAvailableHeight(treeViewItem);

            var actual = new Size(availableSize.Width, availableSize.Height);
            foreach (UIElement child in InternalChildren)
            {
                FlexPanel.SetAvailableHeight(child, availableSize.Height);
                FlexPanel.SetAvailableWidth(child, availableSize.Width);

                child.Measure(actual);
                var size = child.DesiredSize;

            }
            if (double.IsInfinity(availableSize.Width))
            {
                return new Size(0, 0);
            }
            return actual;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            base.ArrangeOverride(finalSize);
            foreach (UIElement child in InternalChildren)
            {

                var sizeRect = new Rect(0, 0, finalSize.Width, finalSize.Height);
                CustomPanel.SetArrangeRect(child, sizeRect);
                child.Arrange(sizeRect);
   
            }
            return finalSize;
        }

    }
}
