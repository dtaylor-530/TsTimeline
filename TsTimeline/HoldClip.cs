using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TsTimeline
{
    [TemplatePart(Name="PART_LEFT", Type=typeof(Thumb))]
    [TemplatePart(Name="PART_RIGHT", Type=typeof(Thumb))]
    [TemplatePart(Name="PART_CENTER", Type=typeof(Thumb))]
    public partial class ClipBase
    {
        private Thumb _left;
        private Thumb _right;
        private Thumb _center;

        public static readonly DependencyProperty StartValueProperty =
            DepProp.Register<ClipBase, double>(
                nameof(StartValue),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                ValueChanged);

        public static readonly DependencyProperty EndValueProperty =
            DepProp.Register<ClipBase, double>(
                nameof(EndValue),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                ValueChanged);

        public double StartValue
        {
            get => (double) GetValue(StartValueProperty);
            set => SetValue(StartValueProperty, value);
        }

        public double EndValue
        {
            get => (double) GetValue(EndValueProperty);
            set => SetValue(EndValueProperty, value);
        }

        private double MaxValue => (int) (ActualWidth * (1.0 / Viewport.ZoomX) + 0.5d);

        private void Right_OnDragDelta(Vector vector)
        {
            if (IsReadOnly)
                return;
            
            var change = Math.Ceiling(vector.X * (1.0d / Viewport.ZoomX) - 0.5d);

            // 右側のクランプ
            if (EndValue + change > MaxValue)
            {
                change = MaxValue - EndValue;
            }
            // 左側のクランプ
            else if (EndValue + change <= StartValue)
            {
                change =  StartValue - EndValue + 1;
            }
            EndValue += change;
        }

        private void Center_OnDragDelta(Vector vector)
        {
            if (IsReadOnly)
                return;
            var change = Math.Ceiling(vector.X * (1.0d / Viewport.ZoomX) - 0.5d);            
            var diff = ClampToCanvasDiff(change);

            StartValue += diff;
            EndValue += diff;
        }

        private void Left_OnDragDelta(Vector vector)
        {
            if (IsReadOnly)
                return;
            
            var change = Math.Ceiling(vector.X * (1.0d / Viewport.ZoomX) - 0.5d);
            // 右側のクランプ
            if (StartValue + change >= EndValue)
            {
                change = EndValue - StartValue - 1;
            }
            // 左側のクランプ
            else if (StartValue + change < 0)
            {
                change = -StartValue;
            }

            StartValue += change;
        }

        public double ClampToCanvasDiff(double d)
        {
            if (StartValue + d <= 0)
                return -StartValue;

            if (EndValue + d >= MaxValue)
            {
                return MaxValue - EndValue;
            }

            return d;
        }

        private bool TrySetupThumbs()
        {
            if (_left != null && _right != null && _center != null)
                return true;
            
            if (_left is null)
                _left = this.GetTemplateChild("PART_LEFT") as Thumb;
            if (_right is null)
                _right = this.GetTemplateChild("PART_RIGHT") as Thumb;
            if (_center is null)
                _center = this.GetTemplateChild("PART_CENTER") as Thumb;

            var result = _left != null && _right != null && _center != null;

            if (result)
            {
                var leftBinder = new ThumbDragToMousePointConverter(_left,OnMouseDownSelectedChanged);
                leftBinder.BindDragDelta(Left_OnDragDelta);
                
                var rightBinder = new ThumbDragToMousePointConverter(_right,OnMouseDownSelectedChanged);
                rightBinder.BindDragDelta(Right_OnDragDelta);
                
                var centerBinder = new ThumbDragToMousePointConverter(_center,OnMouseDownSelectedChanged);
                centerBinder.BindDragDelta(Center_OnDragDelta);
                
                Loaded += (s, e) =>
                {
                    UpdateThumbs();
                };
            }
            
            return result;
        }
        
        private void UpdateThumbs()
        {
            if (TrySetupThumbs() is false)
                return;
            
            Canvas.SetLeft(_left,StartValue * Viewport.Scale * Viewport.ZoomX - _left.ActualWidth / 2);
            Canvas.SetLeft(_right,EndValue * Viewport.Scale * Viewport.ZoomX - _right.ActualWidth / 2);
            Canvas.SetLeft(_center,StartValue * Viewport.Scale * Viewport.ZoomX);
            
            var w = EndValue * Viewport.Scale * Viewport.ZoomX - StartValue * Viewport.Scale * Viewport.ZoomX;

            if (w > 0)
                _center.Width = w;
        }
    }
}