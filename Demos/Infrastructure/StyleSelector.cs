
namespace Demos
{
    internal class CustomStyleSelector : StyleSelector
    {
        public ChartType ChartType { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ViewModel { Key: Keys.TriggerClip })
            {
                return TriggerStyle;
            }
            if (item is Notification { Key: Keys.TrackClip })
            {
                return TrackStyle;
            }
            if (item is Notification { Key: Keys.Title })
            {
                return TitleStyle;
            }
            if (item is Notification { Key: Keys.BandClip })
            {
                return BandStyle;
            }

            if (item is Notification { Key: Keys.Master })
            {
                return MasterStyle;
            }

            if (item is Notification { Key: Keys.Progress })
            {
                return ProgressStyle;
            }
            if (item is Notification { Key: Keys.Configuration })
            {
                return ConfigStyle;
            }

            if (item is Notification { Key: Keys.Chart })
            {
                return ChartStyle;
            }

            if (item is Notification { Key: Keys.ChartObjects })
            {
                return ChartObjectsStyle;
            }

            if (item is Notification { Key: Keys.Charts })
            {
                return ChartsStyle;
            }

            if (item is Notification { Key: Keys.Playlist })
            {
                return PlaylistStyle;
            }
            if (item is Notification { Key: Keys.TimelineThumb })
            {
                return TimelineThumbStyle;
            }
            if (item is Notification { Key: Keys.HoldclipThumb })
            {
                return HoldClipThumbStyle;
            }
            if (item is Notification { Key: Keys.Line })
            {
                return LineStyle;
            }
            if (item is Notification { Key: Keys.Area })
            {
                return AreaStyle;
            }
            if (item is Notification { Key: Keys.ChartLine })
            {
                return ChartLineStyle;
            }

            if (item is ViewModel { Key: Keys.Point })
            {
                return PointStyle;
            }

            if (item is Country country)
            {
                if (country.Group == Groups.One)
                {
                    return CountryStyle;
                }
                if (country.Group == Groups.Two)
                {
                    return CountryItemStyle;
                }
            }
            if (item is ChildrenConverter.CountryName)
            {
                return ContentStyle;
            }
            if (item is ChildrenConverter.Flag)
            {
                return ContentStyle;
            }

            if (item is ViewModel { Key: Keys.HoldClip } _viewModel)
            {
                if (_viewModel.Group is Groups.One)
                {
                    return BandStyle;
                }
                return HoldStyle;
            }

            if (item is ViewModel { Key: Keys.Player })
            {
                return DefaultStyle ?? base.SelectStyle(item, container);
            }
            if (item is ViewModel { Key: Keys.Speed })
            {
                return DefaultStyle ?? base.SelectStyle(item, container);
            }
            if (item is ViewModel { Key: Keys.ChartType })
            {
                return DefaultStyle ?? base.SelectStyle(item, container);
            }
            if (item is Notification { Key: Keys.Renderer })
            {
                return RendererStyle;
            }
            if (item is ViewModel { Key: Keys.Viewports })
            {
                return DefaultStyle ?? base.SelectStyle(item, container);
            }
            if (item is Notification { Key: Keys.Viewport })
            {
                return DefaultStyle ?? base.SelectStyle(item, container);
            }
            if (item is Notification { Key: Keys.Time })
            {
                return ContentStyle;
            }

            return DefaultStyle ?? base.SelectStyle(item, container);
        }

        public Style HoldStyle { get; set; }
        public Style TriggerStyle { get; set; }
        public Style TrackStyle { get; set; }
        public Style BandStyle { get; set; }
        public Style PointStyle { get; set; }
        public Style Point2Style { get; set; }
        public Style CountryStyle { get; set; }
        public Style CountryItemStyle { get; set; }
        public Style ContentStyle { get; set; }
        public Style DefaultStyle { get; set; }
        public Style MasterStyle { get; set; }
        public Style ConfigStyle { get; set; }
        public Style ChartStyle { get; set; }
        public Style ChartObjectsStyle { get; set; }
        public Style ChartsStyle { get; set; }
        public Style ProgressStyle { get; set; }
        public Style PlaylistStyle { get; set; }
        public Style AreaStyle { get; set; }
        public Style Area2Style { get; set; }
        public Style LineStyle { get; set; }
        public Style TimelineThumbStyle { get; set; }
        public Style HoldClipThumbStyle { get; set; }
        public Style RendererStyle { get; set; }
        public Style ChartLineStyle { get; set; }
        public Style TitleStyle { get; set; }
    }
}
