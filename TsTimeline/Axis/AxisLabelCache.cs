
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace TsTimeline
{
    public sealed class AxisLabelCache
    {
        private readonly Dictionary<string, FormattedText> _cache = new();

        private readonly Typeface _typeface;
        private readonly double _fontSize;
        private readonly Brush _foreground;

        public AxisLabelCache(
            Typeface typeface,
            double fontSize,
            Brush foreground)
        {
            _typeface = typeface;
            _fontSize = fontSize;
            _foreground = foreground;
        }

        public FormattedText Get(string text)
        {
            if (_cache.TryGetValue(text, out var existing))
                return existing;

            var formatted = new FormattedText(
                text,
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                _typeface,
                _fontSize,
                _foreground,
                VisualTreeHelper.GetDpi(Application.Current.MainWindow).PixelsPerDip);

            _cache[text] = formatted;

            return formatted;
        }

        public void Clear()
        {
            _cache.Clear();
        }
    }
}