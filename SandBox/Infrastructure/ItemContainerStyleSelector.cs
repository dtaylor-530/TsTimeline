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
            if (item is TriggerClipVm)
            {
                return TriggerStyle;
            }

            if (item is HoldClipVm)
            {
                return HoldStyle;
            }
            return base.SelectStyle(item, container);
        }

        public Style HoldStyle { get; set; }
        public Style TriggerStyle { get; set; }
    }
}
