using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TsTimeline
{
    public partial class ClipBase
    {
        private FrameworkElement? point;

        public static readonly DependencyProperty XProperty =
    DependencyProperty.Register(nameof(X), typeof(double), typeof(ClipBase), new PropertyMetadata(0d, updateX));



        public static readonly DependencyProperty YProperty =
    DependencyProperty.Register(nameof(Y), typeof(double), typeof(ClipBase), new PropertyMetadata(0d, updateY));

        private static void updateX(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClipBase clipBase && e.NewValue is double value)
            {
                clipBase.startValue = clipBase.X;
                clipBase.endValue = clipBase.X + clipBase.Size.Width ;
                clipBase.updateX();
            }
        }
        private static void updateSize(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClipBase clipBase && e.NewValue is Size value)
            {
                clipBase.startValue = clipBase.X;
                clipBase.endValue = clipBase.X + clipBase.Size.Width;
                clipBase.updateX();
            }
        }

        private static void updateY(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClipBase clipBase && e.NewValue is double value)
            {
                clipBase.updateY();
            }
        }

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



        public Size Size
        {
            get { return (Size)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(nameof(Size), typeof(Size), typeof(ClipBase), new PropertyMetadata(updateSize));



        private void updatePointX()
        {
            point ??= this.GetTemplateChild("PART_POINT") as FrameworkElement;
            if (point == null)
                return;

            Canvas.SetLeft(this, X * ViewportX.Zoom);

        }

        private void updatePointY()
        {
            point ??= this.GetTemplateChild("PART_POINT") as FrameworkElement;
            if (point == null)
                return;
            Canvas.SetTop(this, ViewportY.ViewportLength - Y * ViewportY.Zoom);
        }
    }
}
