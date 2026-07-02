using System;
using System.Collections.Generic;
using System.Text;

namespace Renderers
{
    public sealed class TimeLabelFormatter : IFormatter
    {
        public string Format(object value)
        {
            if (value is not double _value)
            { 
                throw new Exception("DS sd3 g"); 
            }

            var ts = TimeSpan.FromSeconds(_value);

            if (ts.TotalHours >= 1)
                return ts.ToString(@"hh\:mm\:ss");

            if (ts.TotalMinutes >= 1)
                return ts.ToString(@"mm\:ss");

            return ts.ToString(@"ss\.fff");
        }
    }
}
