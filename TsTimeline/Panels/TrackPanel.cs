using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TsTimeline
{
    public class TrackPanel : Canvas
    {

    }

    public class TimelinePanel : Canvas
    {

        protected override Size MeasureOverride(Size availableSize)
        {
            var treeViewItem = VisualTreeExtensions.FindParent<TreeViewItem>(this);
            var available = FlexPanel.GetAvailableHeight(treeViewItem);

            foreach (UIElement child in InternalChildren)
            {
                FlexPanel.SetAvailableHeight(child, availableSize.Height);
                FlexPanel.SetAvailableWidth(child, availableSize.Width);

                child.Measure(new Size(availableSize.Width, availableSize.Height));
                var size = child.DesiredSize;

            }

            return new Size(
                double.IsInfinity(availableSize.Width)? 0: availableSize.Width, 
                double.IsInfinity(availableSize.Height) ? 0 : availableSize.Height);
        }
    }
}
