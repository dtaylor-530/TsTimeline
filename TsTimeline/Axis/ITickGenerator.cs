using System;
using System.Collections.Generic;
using System.Text;

namespace TsTimeline
{
    public interface ITickGenerator
    {
        IEnumerable<AxisTick> Generate(
            Viewport viewport);
    }
}
