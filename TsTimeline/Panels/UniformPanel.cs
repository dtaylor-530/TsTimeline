using System.Windows;
using System.Windows.Controls;

namespace TsTimeline
{
    public class UniformPanel : Panel
    {
        protected override Size MeasureOverride(Size constraint)
        {
            var treeViewItem = VisualTreeExtensions.FindParent<TreeViewItem>(this);
            var available = FlexPanel.GetAvailableHeight(treeViewItem);

            var childAv = constraint.Height / InternalChildren.Count;
            foreach (UIElement child in InternalChildren)
            {

                FlexPanel.SetAvailableHeight(child, childAv);
                //child.InvalidateMeasure();
                child.Measure(new Size(constraint.Width, childAv));
                var desiredSize = child.DesiredSize;
                //sum =
                //    new Size(Math.Max(sum.Width, child.DesiredSize.Width), sum.Height + child.DesiredSize.Height);
            }

            return new Size(treeViewItem.ActualWidth, treeViewItem.ActualHeight);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            //var x = base.ArrangeOverride(arrangeSize);
            var treeViewItem = VisualTreeExtensions.FindParent<TreeViewItem>(this);
            var available = FlexPanel.GetAvailableHeight(treeViewItem);
            var childAvailable = available / InternalChildren.Count;
            double offset = 0;
            foreach (UIElement child in InternalChildren)
            {
                child.Arrange(new Rect(0, offset, arrangeSize.Width, childAvailable));
                offset += childAvailable;


            }
            return arrangeSize;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
        }

    }
}
