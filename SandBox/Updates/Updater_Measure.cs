using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace SandBox
{
    public partial class ViewModel
    {
        private readonly AxisLabelCache _labelCache = new(new Typeface("Segoe UI"), 10, Brushes.Black);


        void render_Measure(ClipBase element, Notification dataContext, Viewport viewport, DrawingContext drawingContext, ViewModel playlist)
        {
            if (dataContext is ViewModel { Axis: { } axis, Group: { } group } && axis == viewport.Axis && group == viewport.Group)
            {
            }
            else
                return;

            if (element.ActualHeight == 0 || element.ActualWidth == 0)
                return;
            //var formatter = ValueConverter != null
            //         ? (ILabelFormatter)new ConverterFormatter(ValueConverter)
        
            IEnumerable<AxisTick> ticks = null;

            if (viewport.Axis == Axis.X)
                ticks = new TimelineTickGenerator().Generate(viewport);
            else if (viewport.Axis == Axis.Y && viewport.Group == Groups.One)
                ticks = new ReverseTimelineTickGenerator().Generate(viewport);
            else if (viewport.Axis == Axis.Y && viewport.Group == Groups.Two)
                ticks = new TrackTickGenerator().Generate((playlist, viewport));


            var context = new AxisRenderContext
            {
                DrawingContext = drawingContext,
                Ticks = ticks,
                Bounds = new Rect(0, 0, element.ActualWidth, element.ActualHeight),
                LabelCache = _labelCache,
            };

            foreach (var axisLayer in dataContext.Children.OfType<IAxisLayer>())
            {
                axisLayer.Render(context);
            }

        }
    }

    class TrackTickGenerator : ITickGenerator
    {
        public IEnumerable<AxisTick> Generate(object context)
        {
            if(context is (ViewModel viewModel, Viewport viewport))
            {
                int i = 0;
                foreach (ViewModel child in viewModel.Children)
                {
                    yield return new AxisTick(child.Name, viewport.MinimumSpacing * i * viewport.Zoom, TickLevel.Major);
                    i++;
                }
            }
        }
    }
}
