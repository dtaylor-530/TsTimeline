using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TsTimeline
{
    public partial class ClipBase
    {
        private FrameworkElement? point;

        public static readonly DependencyProperty XProperty =
    DependencyProperty.Register(nameof(X), typeof(double), typeof(ClipBase), new PropertyMetadata(0d));

        public static readonly DependencyProperty YProperty =
    DependencyProperty.Register(nameof(Y), typeof(double), typeof(ClipBase), new PropertyMetadata(0d));
     

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }


        private void updatePoint()
        {
            point ??= this.GetTemplateChild("PART_POINT") as FrameworkElement;
            if (point == null)
                return;

            Canvas.SetLeft(this, X * ViewportX.Scale * ViewportX.Zoom);
            Canvas.SetTop(this, ViewportY.ViewportLength - Y * ViewportY.Scale * ViewportY.Zoom);
        }
    }
}
