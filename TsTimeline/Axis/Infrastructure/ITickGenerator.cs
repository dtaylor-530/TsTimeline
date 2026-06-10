using System.Collections.Generic;

namespace TsTimeline
{
    public interface ITickGenerator
    {
        IEnumerable<AxisTick> Generate(Viewport viewport);
    }
}
