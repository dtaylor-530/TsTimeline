using System;
using System.Collections.Generic;

namespace TsTimeline
{
    public sealed class TimelineTickGenerator : ITickGenerator
    {
        public IEnumerable<AxisTick> Generate(Viewport viewport)
        {
            if (viewport.ZoomX <= 0)
                yield break;

            double unitStep =
                (viewport.MinPixelSpacing / viewport.ZoomX) / viewport.ScaleX;

            double majorStep =
                NiceNumber(unitStep * 5);

            double minorStep =
                majorStep / 5;

            double start =
                Math.Floor(viewport.VisibleStart / minorStep)
                * minorStep;

            double end =
                viewport.VisibleEnd;

            int count = (int)Math.Round((end - start) / minorStep);

            for (int i = 0; i <= count; i++)
            {
                double v = start + i * minorStep;
                yield return new AxisTick(
                    v,
                    viewport.WorldToXScreen(v),
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

    public sealed class YDown_TimelineTickGenerator : ITickGenerator
    {
        public IEnumerable<AxisTick> Generate(Viewport viewport)
        {
            if (viewport.ZoomY <= 0)
                yield break;

            double unitStep =
                (viewport.MinPixelSpacing / viewport.ZoomY) / viewport.ScaleY;

            double majorStep =
                NiceNumber(unitStep * 5);

            double minorStep =
                majorStep / 5;

            double start =
                Math.Floor(viewport.VisibleStart / minorStep)
                * minorStep;

            double end =
                viewport.VisibleEnd;

            int count = (int)Math.Round((end - start) / minorStep);

            for (int i = 0; i <= count; i++)
            {
                double v = start + i * minorStep;
                yield return new AxisTick(
                    v,
                    viewport.WorldToYScreen(v),
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

    public sealed class YUp_TimelineTickGenerator : ITickGenerator
    {
        public IEnumerable<AxisTick> Generate(Viewport viewport)
        {
            if (viewport.ZoomY <= 0)
                yield break;

            double unitStep =
                (viewport.MinPixelSpacing / viewport.ZoomY) / viewport.ScaleY;

            double majorStep =
                NiceNumber(unitStep * 5);

            double minorStep =
                majorStep / 5;

            double start =
                Math.Floor(0 / minorStep)
                * minorStep;

            double end =
                viewport.VisibleEnd;

            int count = (int)Math.Round((end - start) / minorStep);

            for (int i = 0; i <= count; i++)
            {
                double v = start + i * minorStep;
                var y = viewport.ViewportHeight - viewport.WorldToYScreen(v);
                if (y > 0)
                    yield return new AxisTick(v, y,                        
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