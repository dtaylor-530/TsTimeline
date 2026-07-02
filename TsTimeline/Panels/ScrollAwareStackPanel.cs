using System.Windows;

namespace TsTimeline
{
    public partial class CustomPanel
    {
        protected Size ScrollAwareStackPanel_MeasureOverride(Size availableSize)
        {
            throw new System.Exception("!F ds");
            //var size = base.MeasureOverride(availableSize);
            //double sumHeight = 0;
            //foreach(FrameworkElement child in Children)
            //{
            //    child.Measure(new Size(availableSize.Width, double.PositiveInfinity));
            //    sumHeight += child.DesiredSize.Height;
            //}
            //return new Size(ViewportX?.Length * ViewportX?.Zoom ?? size.Width, sumHeight); ;
        }
    }
}
