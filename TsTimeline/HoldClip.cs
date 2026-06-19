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
        private Grid? _grid;
        private double startValue, endValue;

        public sealed class RangeChangedEventArgs : RoutedEventArgs
        {
            public double Start { get; }
            public double End { get; }

            public RangeChangedEventArgs(
                RoutedEvent routedEvent,
                object source,
                double start,
                double end)
                : base(routedEvent, source)
            {
                Start = start;
                End = end;
            }
        }

        //private double MaxValue => (int)(ActualWidth * (1.0 / ViewportX.Zoom) + 0.5d);

        private void updateThumbs()
        {
            if (TrySetupThumbs() is false)
                return;

            var width = (endValue - startValue);


            if (width > 0)
            {
                this.Width = _center.Width = width * ViewportX.Zoom;
            }
            else
            {
            }
            if (X != startValue)
                this.X = startValue;
            if (Size.Width != width)

                this.Size = new Size(width, this.Size.Height);

            this.RaiseEvent(
        new RangeChangedEventArgs(
            RangeChangedEvent,
            this,
            startValue,
            endValue));
            //}
        }

        public static readonly RoutedEvent RangeChangedEvent =
    EventManager.RegisterRoutedEvent(
        nameof(RangeChanged),
        RoutingStrategy.Bubble,
        typeof(EventHandler<RangeChangedEventArgs>),
        typeof(ClipBase));

        public event EventHandler<RangeChangedEventArgs> RangeChanged
        {
            add => AddHandler(RangeChangedEvent, value);
            remove => RemoveHandler(RangeChangedEvent, value);
        }

        bool TrySetupThumbs()
        {
            if (_left != null && _right != null && _center != null)
                return true;

            _left ??= this.GetTemplateChild("PART_LEFT") as Thumb;
            _right ??= this.GetTemplateChild("PART_RIGHT") as Thumb;
            _center ??= this.GetTemplateChild("PART_CENTER") as Thumb;
            _grid ??= this.GetTemplateChild("PART_GRID") as Grid;

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

                var change = vector.X;
                //if (EndValue + change > MaxValue)
                //{
                //    change = MaxValue - EndValue;
                //}
                //else if (EndValue + change <= StartValue)
                //{
                //    change = StartValue - EndValue + 1;
                //}
                //X += change;
                endValue += change / ViewportX.Zoom;
                updateX();
                //this.Size = new Size(this.Size.Width + change, this.Size.Height);
            }

            void center_OnDragDelta(Vector vector)
            {
                var change = vector.X;
                //if (StartValue + change < 0)
                //{
                //    change = 0 - StartValue;
                //}

                startValue += change / ViewportX.Zoom;
                endValue += change / ViewportX.Zoom;
                updateX();
            }

            void left_OnDragDelta(Vector vector)
            {
                if (IsReadOnly)
                    return;


                var change = vector.X;

                //if (StartValue + change >= EndValue)
                //{
                //    change = EndValue - StartValue - 1;
                //}
                //else if (StartValue + change < 0)
                //{
                //    change = -StartValue;
                //}
                //X += change;
                startValue += change / ViewportX.Zoom;
                updateX();
                //this.Size = new Size(this.Size.Width - change, this.Size.Height);               
            }

            return result;
        }
    }
}