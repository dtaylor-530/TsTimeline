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
        
        private void updateBand()
        {
            center ??= this.GetTemplateChild("PART_CENTER") as FrameworkElement;
            if (center == null)
                return;
            var width = Size.Width * ViewportX.Zoom;

            Canvas.SetLeft(this, startValue * ViewportX.Zoom);

            //if (w > 0)
            this.Width = width;        
           
        }
    }
}
