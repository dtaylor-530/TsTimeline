namespace Renderers
{
    public sealed class ConverterFormatter(IValueConverter _converter) : IFormatter
    {
        public string Format(object value)
        {
            return
                _converter.Convert(
                    value,
                    typeof(string),
                    null,
                    null)?.ToString()
                ?? value.ToString();
        }
    }
}
