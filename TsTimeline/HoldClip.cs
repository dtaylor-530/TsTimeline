using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TsTimeline
{
    [TemplatePart(Name = "PART_LEFT", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_RIGHT", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_CENTER", Type = typeof(Thumb))]
    public partial class ClipBase
    {
        private Thumb? _left, _right, _center;

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
            get => (double)GetValue(StartValueProperty);
            set => SetValue(StartValueProperty, value);
        }

        public double EndValue
        {
            get => (double)GetValue(EndValueProperty);
            set => SetValue(EndValueProperty, value);
        }

        private double MaxValue => (int)(ActualWidth * (1.0 / Viewport.ZoomX) + 0.5d);

        private void updateThumbs()
        {
            if (TrySetupThumbs() is false)
                return;

            Canvas.SetLeft(_left, StartValue * Viewport.ScaleX * Viewport.ZoomX - _left.ActualWidth / 2);
            Canvas.SetLeft(_right, EndValue * Viewport.ScaleX * Viewport.ZoomX - _right.ActualWidth / 2);
            Canvas.SetLeft(_center, StartValue * Viewport.ScaleX * Viewport.ZoomX);

            var w = EndValue * Viewport.ScaleX * Viewport.ZoomX - StartValue * Viewport.ScaleX * Viewport.ZoomX;

            if (w > 0)
                _center.Width = w;
        }

        bool TrySetupThumbs()
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
                var leftBinder = new ThumbDragToMousePointConverter(_left, OnMouseDownSelectedChanged);
                leftBinder.BindDragDelta(left_OnDragDelta);

                var rightBinder = new ThumbDragToMousePointConverter(_right, OnMouseDownSelectedChanged);
                rightBinder.BindDragDelta(right_OnDragDelta);

                var centerBinder = new ThumbDragToMousePointConverter(_center, OnMouseDownSelectedChanged);
                centerBinder.BindDragDelta(center_OnDragDelta);

                //Loaded += (s, e) =>
                //{
                //    UpdateThumbs();
                //};
            }

            void right_OnDragDelta(Vector vector)
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
                    change = StartValue - EndValue + 1;
                }
                EndValue += change;
            }

            void center_OnDragDelta(Vector vector)
            {
                if (IsReadOnly)
                    return;
                var change = Math.Ceiling(vector.X * (1.0d / Viewport.ZoomX) - 0.5d);
                var diff = clampToCanvasDiff(change);

                StartValue += diff;
                EndValue += diff;
            }

            void left_OnDragDelta(Vector vector)
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

            double clampToCanvasDiff(double d)
            {
                if (StartValue + d <= 0)
                    return -StartValue;

                if (EndValue + d >= MaxValue)
                {
                    return MaxValue - EndValue;
                }

                return d;
            }

            return result;
        }
    }
}