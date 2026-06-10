using System;
using System.Collections.Generic;
using System.Text;

namespace TsTimeline
{
    public sealed class AxisFactory(ITickGenerator tickGenerator)
    {
        public AxisModel Build(Viewport viewport)
        {
            var model = new AxisModel
            {
                VisibleStart = viewport.VisibleStart,
                VisibleEnd = viewport.VisibleEnd,
                PixelsPerUnit = viewport.ZoomX
            };

            model.Ticks.AddRange(tickGenerator.Generate(viewport));

            return model;
        }
    }
}
