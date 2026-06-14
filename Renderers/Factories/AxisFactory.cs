namespace Renderers
{
    public sealed class X_AxisFactory(ITickGenerator tickGenerator) : IAxisFactory
    {
        public AxisModel Build(Viewport viewport)
        {
            var model = new AxisModel
            {
                VisibleStart = viewport.VisibleStart,
                VisibleEnd = viewport.VisibleEnd,
                PixelsPerUnit = viewport.ScaleX * viewport.ZoomX
            };

            model.Ticks.AddRange(tickGenerator.Generate(viewport));

            return model;
        }
    }

    public sealed class Y_AxisFactory(ITickGenerator tickGenerator) : IAxisFactory
    {
        public AxisModel Build(Viewport viewport)
        {
            var model = new AxisModel
            {
                VisibleStart = 0,
                VisibleEnd = 10,
                PixelsPerUnit = viewport.ScaleY * viewport.ZoomY
            };

            model.Ticks.AddRange(tickGenerator.Generate(viewport));

            return model;
        }
    }
}
