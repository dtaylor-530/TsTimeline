namespace Renderers
{
    public sealed class AxisLabelCache(Typeface typeface, double fontSize, Brush foreground)
    {
        private readonly Dictionary<string, FormattedText> _cache = new();

        public FormattedText Get(string text)
        {
            if (_cache.TryGetValue(text, out var existing))
                return existing;

            var formatted = new FormattedText(
                text,
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                typeface,
                fontSize,
                foreground,
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