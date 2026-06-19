using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Renderers;

namespace TsTimeline
{
    public class ValueChangedArgs : RoutedEventArgs
    {
        public double NewValue { get; }
        public ValueChangedArgs(RoutedEvent routedEvent, double newValue)
            : base(routedEvent)
        {
            NewValue = newValue;
        }
    }

    public class Timeline : Control
    {
        private Thumb? _thumb;
        private System.Windows.Shapes.Rectangle? _rectangle;
        private System.Windows.Shapes.Rectangle? _line;

        private double _targetValue;
        private double _smoothValue;
        private bool _renderingAttached;

        private const string Part_Line = "PART_LINE";
        private const string Part_Thumb = "PART_THUMB";
        private const string part_Area = "PART_AREA";

        static Timeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(Timeline),
                new FrameworkPropertyMetadata(typeof(Timeline)));
        }

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(Timeline));

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(nameof(Direction), typeof(Direction), typeof(Timeline), new PropertyMetadata(Direction.Down, changed));

        private static void changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timeline = (Timeline)d;
            var self = (Timeline)d;
            self._smoothValue = 0;
            timeline.UpdateVisuals(self._smoothValue);
        }

        public Direction Direction
        {
            get { return (Direction)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        protected void OnMyClick()
        {
            RoutedEventArgs newEventArgs = new ValueChangedArgs(ValueChangedEvent, Value);
            RaiseEvent(newEventArgs);
        }


        public static readonly DependencyProperty ViewportProperty =
            DependencyProperty.Register(
                nameof(Viewport),
                typeof(Viewport),
                typeof(Timeline),
                new PropertyMetadata(null, OnViewportChanged));

        public Viewport? Viewport
        {
            get => (Viewport?)GetValue(ViewportProperty);
            set => SetValue(ViewportProperty, value);
        }

        private static void OnViewportChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var self = (Timeline)d;
            self._smoothValue = 0;
            if (e.OldValue is Viewport old)
                old.PropertyChanged -= self.OnViewportPropertyChanged;

            if (e.NewValue is Viewport @new)
                @new.PropertyChanged += self.OnViewportPropertyChanged;

            self.UpdateVisuals(self._smoothValue);
        }

        private void OnViewportPropertyChanged(
            object? sender,
            System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Viewport.Zoom):
                case nameof(Viewport.ViewportLength):
                case nameof(Viewport.Offset):
                    UpdateVisuals(_smoothValue);
                    break;
            }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(double),
                typeof(Timeline),
                new FrameworkPropertyMetadata(
                    0d,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static void OnValueChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var self = (Timeline)d;
            self._targetValue = (double)e.NewValue;
            self.AttachRenderingLoop();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _thumb = null;
            _rectangle = null;
            _line = null;

            TryGetThumb(out _);
            TryGetRectangle(out _);
            TryGetLine(out _);
            UpdateVisuals(_smoothValue);
        }

        private void AttachRenderingLoop()
        {
            if (_renderingAttached) return;
            _renderingAttached = true;
            CompositionTarget.Rendering += OnRendering;
        }

        private void OnRendering(object? sender, EventArgs e)
        {
            const double smoothing = 0.18;
            const double threshold = 0.01;

            _smoothValue += (_targetValue - _smoothValue) * smoothing;


            UpdateVisuals(_smoothValue);

            if (Math.Abs(_targetValue - _smoothValue) < threshold)
            {
                // Snap to exact target and stop the loop
                _smoothValue = _targetValue;
                UpdateVisuals(_smoothValue);

                CompositionTarget.Rendering -= OnRendering;
                _renderingAttached = false;
            }
        }

        private void UpdateVisuals(double worldValue)
        {
            if (Direction == Direction.None)
            {
                Visibility = Visibility.Collapsed;
                return;
            }
            Visibility = Visibility.Visible;
            if (Viewport == null || Viewport.Zoom <= 0)
                return;

            double position = worldValue - Viewport.Offset;

            if (position < 0)
            {
                this.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.Visibility = Visibility.Visible;
            }
            // Reverse position for Up/Left directions
            //if (IsReversed)
            //{
            //    position = IsHorizontal
            //        ? ActualWidth - position
            //        : ActualHeight - position;
            //}

            if (ActualWidth == 0 || ActualHeight == 0)
                return;

            if (TryGetThumb(out var thumb))
            {
         
                if (IsHorizontal)
                {
                    Canvas.SetLeft(thumb, position - thumb.ActualWidth / 2);
                    //Canvas.SetBottom(thumb, 0);
                    if (IsReversed)
                        Canvas.SetTop(thumb, ActualHeight - 15);
                    else
                        Canvas.SetTop(thumb, 0);
                }
                else
                {
                    Canvas.SetTop(thumb, position - thumb.ActualHeight / 2);
                    Canvas.SetLeft(thumb, 0);
                }
            }

            if (TryGetRectangle(out var rectangle))
            {    
                if (IsHorizontal)
                {
                    rectangle.Width = Math.Max(0, position);
                    rectangle.Height = ActualHeight;
                    //if (IsReversed)
                    //    Canvas.SetLeft(rectangle, ActualWidth - rectangle.Width);
                    //else
                    Canvas.SetLeft(rectangle, 0);
                }
                else if (ActualWidth > 0)
                {
                    rectangle.Height = Math.Max(0, position);
                    rectangle.Width = ActualWidth - 16;
                    //if (IsReversed)
                    //    Canvas.SetTop(rectangle, ActualHeight - rectangle.Height);
                    //else
                    Canvas.SetTop(rectangle, 0);
                }
            }

            if (TryGetLine(out var line))
            {
                if (IsHorizontal)
                {
                    line.Width = 1;
                    line.Height = ActualHeight;
                    Canvas.SetLeft(line, position);
                    if (IsReversed)
                    {
                        Canvas.SetTop(line, 0);
                        line.Margin = new(0, 0, 0, 15);
                    }
                    else
                        Canvas.SetTop(line, 15);

                }
                else if (ActualWidth > 0)
                {
                    line.Height = 1;
                    line.Width = ActualWidth - 16;
                    Canvas.SetTop(line, position);
                    if (IsReversed)
                        Canvas.SetRight(line, 15);
                    else
                        Canvas.SetLeft(line, 15);
                }
            }
        }

        private void Thumb_OnDragDelta(Vector vector)
        {
            if (Viewport == null)
                return;

            double worldDelta = IsHorizontal
                ? vector.X
                : vector.Y;

            //if (IsReversed)
            //    worldDelta = -worldDelta;

            Value = Math.Clamp(
                Value + worldDelta,
                Viewport.Start,
                Viewport.End);

            OnMyClick();
        }

        private bool TryGetThumb(out Thumb thumb)
        {
            if (_thumb != null)
            {
                thumb = _thumb;
                return true;
            }

            _thumb = thumb = GetTemplateChild(Part_Thumb) as Thumb;

            if (_thumb != null)
            {
                var binder = new ThumbDragToMousePointConverter(
                    _thumb,
                    OnMouseDownSelectedChanged);

                binder.BindDragDelta(Thumb_OnDragDelta);

                Loaded += (_, _) => UpdateVisuals(_smoothValue);
            }

            return _thumb != null;
        }

        private bool TryGetRectangle(out System.Windows.Shapes.Rectangle rectangle)
        {
            if (_rectangle != null) { rectangle = _rectangle; return true; }
            _rectangle = rectangle = GetTemplateChild(part_Area)
                as System.Windows.Shapes.Rectangle;
            return _rectangle != null;
        }

        private bool TryGetLine(out System.Windows.Shapes.Rectangle line)
        {
            if (_line != null) { line = _line; return true; }
            _line = line = GetTemplateChild(Part_Line)
                as System.Windows.Shapes.Rectangle;
            return _line != null;
        }

        private bool IsHorizontal =>
    Direction == Direction.Up ||
    Direction == Direction.Down;

        private bool IsReversed => Direction == Direction.Up || Direction == Direction.Left;

        protected void OnMouseDownSelectedChanged()
        {
            // Hook for selection service
        }
    }
}
