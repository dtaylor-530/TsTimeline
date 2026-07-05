namespace Renderers
{
    public sealed class NumericLabelFormatter : IFormatter
    {
        public string Format(object value)
        {
            if (value is double _value)
                return _value.ToString("0.###", CultureInfo.InvariantCulture);
            throw new Exception("DS sd3 g");
        }
    }
}
