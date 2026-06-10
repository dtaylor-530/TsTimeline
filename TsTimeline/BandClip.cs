using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TsTimeline
{
    public partial class ClipBase
    {
        private FrameworkElement? center;

        private void UpdateBand()
        {
            center ??= this.GetTemplateChild("PART_CENTER") as FrameworkElement;
            if (center == null)
                return;
            var w = EndValue * Viewport.ScaleX * Viewport.ZoomX - StartValue * Viewport.ScaleX * Viewport.ZoomX;

            Canvas.SetLeft(center, StartValue * Viewport.ScaleX * Viewport.ZoomX);

            //if (w > 0)
                center.Width = w;
            {

            }
           
        }
    }
}
