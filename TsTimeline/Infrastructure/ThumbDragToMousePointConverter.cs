using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace TsTimeline
{
    public class ThumbDragToMousePointConverter
    {
        private readonly Thumb _thumb;

        public ThumbDragToMousePointConverter(
            Thumb thumb,
            Action mouseDown)
        {
            _thumb = thumb;

            _thumb.DragStarted += (s, e) =>
            {
                mouseDown?.Invoke();
            };
        }

        public void BindDragDelta(Action<Vector> function)
        {
            _thumb.DragDelta += (s, e) =>
            {
                function?.Invoke(new Vector(e.HorizontalChange, e.VerticalChange));
            };
        }
    }
}