using System;
using System.Diagnostics;
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

        private double MaxValue => (int)(ActualWidth * (1.0 / ViewportX.Zoom) + 0.5d);

        private void updateThumbs()
        {
            if (TrySetupThumbs() is false)
                return;

            Canvas.SetLeft(this, StartValue * ViewportX.Scale * ViewportX.Zoom);

            var pixelsPerUnit = ViewportX.Scale * ViewportX.Zoom;
            var width = (EndValue - StartValue) * pixelsPerUnit;
            if (width > 0)
            {
                this.Width = _center.Width = width;
            }
            else
            {

            }
        }

        bool TrySetupThumbs()
        {
            if (_left != null && _right != null && _center != null)
                return true;

            _left ??= this.GetTemplateChild("PART_LEFT") as Thumb;
            _right ??= this.GetTemplateChild("PART_RIGHT") as Thumb;
            _center ??= this.GetTemplateChild("PART_CENTER") as Thumb;

            var result = _left != null && _right != null && _center != null;

            if (result)
            {
                var leftBinder = new ThumbDragToMousePointConverter(_left, OnMouseDownSelectedChanged);
                leftBinder.BindDragDelta(left_OnDragDelta);

                var rightBinder = new ThumbDragToMousePointConverter(_right, OnMouseDownSelectedChanged);
                rightBinder.BindDragDelta(right_OnDragDelta);

                var centerBinder = new ThumbDragToMousePointConverter(_center, OnMouseDownSelectedChanged);
                centerBinder.BindDragDelta(center_OnDragDelta);
            }

            void right_OnDragDelta(Vector vector)
            {
                if (IsReadOnly)
                    return;

                var change = vector.X / (ViewportX.Scale * ViewportX.Zoom);
                // 右側のクランプ
                if (EndValue + change > MaxValue)
                {
                    change = MaxValue - EndValue;
                }
                else if (EndValue + change <= StartValue)
                {
                    change = StartValue - EndValue + 1;
                }

                EndValue += change;
            }

            void center_OnDragDelta(Vector vector)
            {
                var change = vector.X / (ViewportX.Scale * ViewportX.Zoom);
                if (StartValue + change < 0)
                {
                    change = 0 - StartValue;
                }
                StartValue += change;
                EndValue += change;
            }

            void left_OnDragDelta(Vector vector)
            {
                if (IsReadOnly)
                    return;


                var change = vector.X / (ViewportX.Scale * ViewportX.Zoom);

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

            return result;
        }
    }
}