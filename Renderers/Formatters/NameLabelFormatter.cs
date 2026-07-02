using System;

namespace Renderers
{
    public sealed class NameLabelFormatter : IFormatter
    {
        public string Format(object value)
        {
            if (value is string _value)
                return _value;
            throw new Exception("DS sd3 g");
        }
    }
}
