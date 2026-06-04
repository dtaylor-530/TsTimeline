using System;
using System.Collections.Generic;
using System.Text;

namespace TsTimeline
{
    public sealed class AxisEngine
    {
        private readonly ITickGenerator _tickGenerator;

        public AxisEngine(
            ITickGenerator tickGenerator)
        {
            _tickGenerator = tickGenerator;
        }

        public AxisModel Build(
            Viewport viewport)
        {
            var model = new AxisModel
            {
                VisibleStart = viewport.VisibleStart,
                VisibleEnd = viewport.VisibleEnd,
                PixelsPerUnit = viewport.ZoomX
            };

            model.Ticks.AddRange(
                _tickGenerator.Generate(viewport));

            return model;
        }
    }
}
