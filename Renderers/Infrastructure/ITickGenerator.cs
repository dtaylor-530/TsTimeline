using System.Collections.Generic;

namespace Renderers
{
    public interface ITickGenerator
    {
        IEnumerable<AxisTick> Generate(Viewport viewport);
    }
}
