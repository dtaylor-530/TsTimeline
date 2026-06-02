using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using TsTimeline;

namespace SandBox
{
    internal class ClipTemplateSelector:DataTemplateSelector
    {

        public override DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            if (item is TrackVm)
            {
                return TrackTemplate;
            }
            return base.SelectTemplate(item, container);
        }
        
        public DataTemplate TrackTemplate { get; set; }
    }
}
