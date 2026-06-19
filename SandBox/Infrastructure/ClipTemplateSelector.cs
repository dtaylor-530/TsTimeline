using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using TsTimeline;
using static SandBox.ChildrenConverter;

namespace SandBox
{
    internal class ClipTemplateSelector : DataTemplateSelector
    {

        public override DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            if (item is TrackViewModel)
            {
                return TrackTemplate;
            }
            if (item is Country)
            {
                return CountryTemplate;
            }
            if (item is CountryName )
            {
                return CountryNameTemplate;
            }
            if (item is Flag )
            {
                return CountryFlagTemplate;
            }
            return base.SelectTemplate(item, container);
        }

        public DataTemplate TrackTemplate { get; set; }

        public DataTemplate CountryTemplate { get; set; }
        public DataTemplate CountryNameTemplate { get; set; }
        public DataTemplate CountryFlagTemplate { get; set; }
    }
}
