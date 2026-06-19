using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using TsTimeline;

namespace SandBox
{
    internal class CustomStyleSelector: StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {             
            if (item is TriggerClipViewModel)
            {
                return TriggerStyle;
            }
              
            if (item is PointViewModel)
            {
                return PointStyle;
            }
            if (item is Country)
            {
                return CountryStyle;
            }

            if (item is HoldClipViewModel)
            {
                var treeView = container.FindVisualParentWithType<TreeView>();
                if (treeView is CombinedTimeline)
                {
                    return BandStyle;
                }
                return HoldStyle;
            }

            return base.SelectStyle(item, container);
        }

        public Style HoldStyle { get; set; }
        public Style TriggerStyle { get; set; }
        public Style BandStyle { get; set; }
        public Style PointStyle { get; set; }
        public Style CountryStyle { get; set; }
    }
}
