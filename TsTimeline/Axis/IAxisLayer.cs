using System;
using System.Collections.Generic;
using System.Text;

namespace TsTimeline
{
    public interface IAxisLayer
    {
        void Render(AxisRenderContext context);
    }
}
