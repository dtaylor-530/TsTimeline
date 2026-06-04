//using System;
//using System.Diagnostics;
//using System.Drawing;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Controls.Primitives;
//using System.Windows.Media;
//using System.Windows.Shapes;

//namespace TsTimeline
//{
    //public class Timeline : Control
    //{
    //    private Thumb _thumb;
    //    private System.Windows.Shapes.Rectangle _rectangle, _line;
    //    private double _targetValue;
    //    private double _smoothValue;
    //    private bool _renderingAttached;
    //    private bool _dirty;

    //    static Timeline()
    //    {
    //        DefaultStyleKeyProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(typeof(Timeline)));
    //    }

    //    private const string Part_Line = "PART_LINE";
    //    private const string Part_Thumb = "PART_THUMB";
    //    private const string Part_Rectangle = "PART_RECTANGLE";



    //    public Viewport Viewport
    //    {
    //        get { return (Viewport)GetValue(ViewportProperty); }
    //        set { SetValue(ViewportProperty, value); }
    //    }

    //    // Using a DependencyProperty as the backing store for Viewport.  This enables animation, styling, binding, etc...
    //    public static readonly DependencyProperty ViewportProperty =
    //        DependencyProperty.Register(nameof(Viewport), typeof(Viewport), typeof(Timeline), new PropertyMetadata(OnViewportChanged));

    //    private static void OnViewportChanged(
    //        DependencyObject d,
    //        DependencyPropertyChangedEventArgs e)
    //    {
    //        var self = (Timeline)d;

    //        if (e.OldValue is Viewport old)
    //            old.PropertyChanged -= self.OnViewportPropertyChanged;

    //        if (e.NewValue is Viewport @new)
    //            @new.PropertyChanged += self.OnViewportPropertyChanged;

    //        self.MarkDirty();
    //    }

    //    private void MarkDirty()
    //    {
    //        _dirty = true;
    //        InvalidateVisual();
    //    }

    //    private void OnViewportPropertyChanged(
    //        object? sender,
    //        System.ComponentModel.PropertyChangedEventArgs e)
    //    {
    //        // Only the properties that affect the axis layout need a rebuild.
    //        // CursorPosition changes are frequent and don't affect tick layout.
    //        switch (e.PropertyName)
    //        {
    //            case nameof(Viewport.OffsetX):
    //            case nameof(Viewport.ZoomX):
    //            case nameof(Viewport.ViewportWidth):
    //            case nameof(Viewport.WorldStart):
    //            case nameof(Viewport.WorldEnd):
    //                MarkDirty();
    //                break;
    //        }
    //    }

    //    public static readonly DependencyProperty ValueProperty =
    //        DepProp.Register<Timeline, double>(
    //            nameof(Value),
    //            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
    //            ValueChanged);

    //    private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        var t = (Timeline)d;
    //        t._targetValue = (double)e.NewValue;
    //        t.AttachRenderingLoop();
    //    }

    //    private void AttachRenderingLoop()
    //    {
    //        if (_renderingAttached)
    //            return;

    //        _renderingAttached = true;
    //        CompositionTarget.Rendering += OnRender;

    //        void OnRender(object sender, EventArgs e)
    //        {
    //            double smoothing = 0.18;

    //            Width = Viewport.ViewportWidth * Viewport.ZoomX;
    //            _smoothValue += (_targetValue - _smoothValue) * smoothing;

    //            UpdateVisuals(_smoothValue);

    //            void UpdateVisuals(double value)
    //            {
    //                if (Viewport.ZoomX <= 0)
    //                    return;

    //                double x = value * Viewport.ZoomX;

    //                if (TryGetThumb(out var thumb))
    //                {
    //                    Canvas.SetLeft(thumb, x - thumb.ActualWidth / 2);
    //                }

    //                if (TryGetRectangle(out var rectangle))
    //                {
    //                    rectangle.Width = x;
    //                }

    //                if (TryGetLine(out var line))
    //                {
    //                    Canvas.SetLeft(line, x - line.ActualWidth / 2);
    //                }
    //            }
    //        }
    //    }

    //    //public double Scale
    //    //{
    //    //    get { return (double)GetValue(ScaleProperty); }
    //    //    set { SetValue(ScaleProperty, value); }
    //    //}

    //    //// Using a DependencyProperty as the backing store for Scale.  This enables animation, styling, binding, etc...
    //    //public static readonly DependencyProperty ScaleProperty =
    //    //    DependencyProperty.Register(nameof(Scale), typeof(double), typeof(Timeline), new PropertyMetadata());



    //    public double Value
    //    {
    //        get => (double)GetValue(ValueProperty);
    //        set => SetValue(ValueProperty, value);
    //    }

    //    public override void OnApplyTemplate()
    //    {
    //        base.OnApplyTemplate();
    //        TryGetThumb(out _);

    //        //TrySetupThumbs();


    //    }

    //    private void Thumb_OnDragDelta(Vector vector)
    //    {
    //        //if (IsReadOnly)
    //        //    return;

    //        var change = Math.Ceiling(vector.X * (1.0d / Viewport.ZoomX) - 0.5d);

    //        if (vector.X > 0)
    //        {
    //            Debug.WriteLine(vector.X);
    //        }
    //        // 右側のクランプ
    //        if (Value + change >= ActualWidth * (1.0d / Viewport.ZoomX))
    //        {
    //            change = ActualWidth * (1.0d / Viewport.ZoomX) - Value;
    //        }
    //        // 左側のクランプ
    //        else if (Value + change <= 0)
    //        {
    //            change = -Value;
    //        }

    //        Value += change;
    //    }


    //    private bool TryGetThumb(out Thumb thumb)
    //    {
    //        if (_thumb != null)
    //        {
    //            thumb = _thumb;
    //            return true;
    //        }

    //        _thumb = thumb = this.GetTemplateChild(Part_Thumb) as Thumb;

    //        if (thumb != null)
    //        {
    //            var eventBinder = new ThumbDragToMousePointConverter(thumb, OnMouseDownSelectedChanged);
    //            eventBinder.BindDragDelta(Thumb_OnDragDelta);
    //            Loaded += (s, e) =>
    //            {
    //                UpdateThumb();
    //            };
    //        }

    //        return _thumb != null;
    //    }

    //    private bool TryGetLine(out System.Windows.Shapes.Rectangle line)
    //    {
    //        if (_line != null)
    //        {
    //            line = _line;
    //            return true;
    //        }

    //        _line = line = this.GetTemplateChild(Part_Line) as System.Windows.Shapes.Rectangle;

    //        return _line != null;
    //    }
    //    private bool TryGetRectangle(out System.Windows.Shapes.Rectangle rectangle)
    //    {
    //        if (_rectangle != null)
    //        {
    //            rectangle = _rectangle;
    //            return true;
    //        }

    //        _rectangle = rectangle = this.GetTemplateChild(Part_Rectangle) as System.Windows.Shapes.Rectangle;
    //        return _rectangle != null;
    //    }

    //    private void UpdateThumb()
    //    {
    //        if (TryGetThumb(out var thumb))
    //        {
    //            Canvas.SetLeft(thumb, Value * Viewport.ZoomX - thumb.ActualWidth / 2);
    //        }
    //        if (TryGetRectangle(out var rectangle))
    //        {
    //            //Canvas.SetLeft(thumb, Value * Scale - thumb.ActualWidth / 2);
    //            rectangle.Width = Value * Viewport.ZoomX;
    //        }
    //        if (TryGetLine(out var line))
    //        {
    //            //Canvas.SetLeft(thumb, Value * Scale - thumb.ActualWidth / 2);
    //            Canvas.SetLeft(line, Value * Viewport.ZoomX - line.ActualWidth / 2);
    //        }
    //    }

    //    protected void OnMouseDownSelectedChanged()
    //    {
    //        //SelectorService.MouseDownSelectionChanged(this);
    //    }
    //}
//}

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

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
        private const string Part_Rectangle = "PART_RECTANGLE";

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
                case nameof(Viewport.Scale):
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

            // Re-resolve all parts — template may have been re-applied
            _thumb = null;
            _rectangle = null;
            _line = null;

            TryGetThumb(out _);
            TryGetRectangle(out _);
            TryGetLine(out _);

            UpdateWidth();
            UpdateVisuals(_smoothValue);
        }


        // Rendering loop (smooth animation toward target)
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

            // WorldToScreen already accounts for OffsetX
            //double screenX = Viewport.WorldToScreen(worldValue);
            double screenX = worldValue;

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
            _rectangle = rectangle = GetTemplateChild(Part_Rectangle)
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
