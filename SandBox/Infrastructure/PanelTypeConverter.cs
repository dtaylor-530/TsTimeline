using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TsTimeline;

namespace SandBox
{
    internal class PanelTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ChartType chartType)
            {
                switch (chartType)
                {
                    case ChartType.Points:
                        return PanelType.Canvas;
                    case ChartType.Bands:
                        return PanelType.DirectionalStackPanel;
                    case ChartType.None:
                        break;
                    default:
                        return PanelType.None;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


