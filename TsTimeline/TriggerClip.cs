using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TsTimeline
{
    [TemplatePart(Name = "PART_THUMB", Type = typeof(Thumb))]
    public partial class ClipBase
    {
        public static readonly DependencyProperty ValueProperty =
            DepProp.Register<ClipBase, double>(
                nameof(Value),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                ValueChanged);

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TryGetThumb(out _);
            TrySetupThumbs();
        }

        private void Thumb_OnDragDelta(Vector vector)
        {
            if (IsReadOnly)
                return;

            var change = Math.Ceiling(vector.X * (1.0d / Viewport.ZoomX) - 0.5d);
            // 右側のクランプ
            if (Value + change >= ActualWidth * (1.0d / Viewport.ZoomX))
            {
                change = ActualWidth * (1.0d / Viewport.ZoomX) - Value;
            }
            // 左側のクランプ
            else if (Value + change <= 0)
            {
                change = -Value;
            }

            Value += change;
        }

        private Thumb _thumb;

        private bool TryGetThumb(out Thumb thumb)
        {
            if (_thumb != null)
            {
                thumb = _thumb;
                return true;
            }

            _thumb = thumb = this.GetTemplateChild("PART_THUMB") as Thumb;

            if (thumb != null)
            {
                var eventBinder = new ThumbDragToMousePointConverter(thumb, OnMouseDownSelectedChanged);
                eventBinder.BindDragDelta(Thumb_OnDragDelta);
                //Loaded += (s, e) =>
                //{
                //    UpdateThumb();
                //};
            }

            return _thumb != null;
        }

        private void UpdateThumb()
        {
            if (TryGetThumb(out var thumb))
            {
                Canvas.SetLeft(thumb, Value * Viewport.ScaleX * Viewport.ZoomX - thumb.ActualWidth / 2);
            }
        }
    }
}