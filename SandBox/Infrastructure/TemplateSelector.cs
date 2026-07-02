using System.Windows;
using System.Windows.Controls;
using static SandBox.ChildrenConverter;

namespace SandBox
{
    internal class ClipTemplateSelector : DataTemplateSelector
    {

        public override DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            if (item is ViewModel { Key: Keys.TrackClip })
            {
                return TrackTemplate;
            }
            if (item is ViewModel { Key: Keys.Speed })
            {
                return SpeedTemplate;
            }
            if (item is ViewModel { Key: Keys.ChartType })
            {
                return ChartTypeTemplate;
            }
            if (item is ViewModel { Key: Keys.Title })
            {
                return TitleTemplate;
            }
            if (item is Notification { Key: Keys.Time })
            {
                return TimeTemplate;
            }
            if (item is Factory )
            {
                return ServiceTemplate;
            }
            if (item is Country)
            {
                return CountryTemplate;
            }
            if (item is CountryName)
            {
                return CountryNameTemplate;
            }
            if (item is Flag)
            {
                return CountryFlagTemplate;
            }
            return base.SelectTemplate(item, container);
        }

        public DataTemplate TrackTemplate { get; set; }
        public DataTemplate CountryTemplate { get; set; }
        public DataTemplate CountryNameTemplate { get; set; }
        public DataTemplate CountryFlagTemplate { get; set; }
        public DataTemplate SpeedTemplate { get; set; }
        public DataTemplate ChartTypeTemplate { get; set; }
        public DataTemplate ServiceTemplate { get; set; }
        public DataTemplate TitleTemplate { get; set; }
        public DataTemplate TimeTemplate { get; set; }
    }
}
