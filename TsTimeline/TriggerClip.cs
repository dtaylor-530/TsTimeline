using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TsTimeline
{
    [TemplatePart(Name = "PART_THUMB", Type = typeof(Thumb))]
    public partial class ClipBase
    {

        private void Thumb_OnDragDelta(Vector vector)
        {
            if (IsReadOnly)
                return;

            var change = vector.X / ViewportX.Zoom;
            if (X + change < 0)
            {
                change = - X;
            }

            X += change;
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

        private void updateThumb()
        {
            if (TryGetThumb(out var thumb))
            {
                Canvas.SetLeft(this, X * ViewportX.Zoom - this.ActualWidth / 2);
            }
        }
    }
}