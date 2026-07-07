namespace Renderers
{
    public sealed class TimelineTickGenerator : ITickGenerator
    {
        public IEnumerable<AxisTick> Generate(object context)
        {
            if (context is not Viewport viewport)
                throw new Exception("EF sdf4");
            if (viewport.Zoom <= 0)
                yield break;

            double unitStep =
                (viewport.MinimumSpacing / viewport.Zoom);

            double majorStep =
                NiceNumber(unitStep * 5);

            double minorStep =
                majorStep / 5;

            double start =
                Math.Floor(viewport.Start + viewport.Offset / minorStep)
                * minorStep;

            double end =
                viewport.End;

            int count = (int)Math.Round((end - start) / minorStep);

            for (int i = 0; i <= count; i++)
            {
                double v = start + i * minorStep;
                yield return new AxisTick(
                    v,
                    viewport.WorldToScreen(v),
                    IsMultiple(v, majorStep)
                        ? TickLevel.Major
                        : TickLevel.Minor);
            }
        }

        private static bool IsMultiple(double v, double step)
        {
            double mod = v % step;
            return Math.Abs(mod) < 1e-9
                || Math.Abs(mod - step) < 1e-9;
        }

        private static double NiceNumber(double value)
        {
            double exponent = Math.Floor(Math.Log10(value));
            double fraction = value / Math.Pow(10, exponent);

            double nice =
                fraction <= 1 ? 1 :
                fraction <= 2 ? 2 :
                fraction <= 5 ? 5 : 10;

            return nice * Math.Pow(10, exponent);
        }
    }
}