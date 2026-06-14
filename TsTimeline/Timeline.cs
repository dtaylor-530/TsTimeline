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

            if (e.OldValue is Viewport old)
                old.PropertyChanged -= self.OnViewportPropertyChanged;

            if (e.NewValue is Viewport @new)
                @new.PropertyChanged += self.OnViewportPropertyChanged;

            self.UpdateWidth();
            self.UpdateVisuals(self._smoothValue);
        }

        private void OnViewportPropertyChanged(
            object? sender,
            System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Viewport.ZoomX):
                case nameof(Viewport.ViewportWidth):
                case nameof(Viewport.ScaleX):
                    {
                       
                        UpdateWidth();
                        UpdateVisuals(_smoothValue);
                        break;
                    }
                case nameof(Viewport.OffsetX):
                    // Offset changes don't affect total canvas width,
                    // but the cursor screen position needs to move.
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

            UpdateWidth();
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


        private void UpdateWidth()
        {
            if (Viewport == null) return;
            Width = Viewport.ViewportWidth;
        }

        private void UpdateVisuals(double worldValue)
        {
            if (Viewport == null || Viewport.ZoomX <= 0) return;

            double screenX = worldValue - Viewport.OffsetX;

            if (TryGetThumb(out var thumb))
                Canvas.SetLeft(thumb, screenX - thumb.ActualWidth / 2);

            if (TryGetRectangle(out var rectangle))
                rectangle.Width = Math.Max(0, screenX);

            if (TryGetLine(out var line))
                Canvas.SetLeft(line, screenX - line.ActualWidth / 2);
        }

        private void Thumb_OnDragDelta(Vector vector)
        {
            if (Viewport == null) return;

            // Convert pixel delta to world units
            double worldDelta = vector.X / Viewport.ZoomX;

            // Clamp to world bounds
            Value = Math.Clamp(
                Value + worldDelta,
                Viewport.WorldStart,
                Viewport.WorldEnd);
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

        protected void OnMouseDownSelectedChanged()
        {
            // Hook for selection service
        }
    }
}
