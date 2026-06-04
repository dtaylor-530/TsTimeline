using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;

namespace TsTimeline
{
    public class ChartControl : FrameworkElement
    {
        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register(
                nameof(Series), typeof(ChartSeries), typeof(ChartControl),
                new FrameworkPropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty ViewportProperty =
            DependencyProperty.Register(
                nameof(Viewport), typeof(Viewport), typeof(ChartControl),
                new FrameworkPropertyMetadata(null, OnViewportChanged));

        public static readonly DependencyProperty PaddingYProperty =
            DependencyProperty.Register(
                nameof(PaddingY), typeof(double), typeof(ChartControl),
                new FrameworkPropertyMetadata(4.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty ShowCursorProperty =
            DependencyProperty.Register(
                nameof(ShowCursor), typeof(bool), typeof(ChartControl),
                new FrameworkPropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty CursorBrushProperty =
            DependencyProperty.Register(
                nameof(CursorBrush), typeof(Brush), typeof(ChartControl),
                new FrameworkPropertyMetadata(Brushes.OrangeRed, OnVisualPropertyChanged));

        public ChartSeries? Series
        {
            get => (ChartSeries?)GetValue(SeriesProperty);
            set => SetValue(SeriesProperty, value);
        }

        public Viewport? Viewport
        {
            get => (Viewport?)GetValue(ViewportProperty);
            set => SetValue(ViewportProperty, value);
        }

        public double PaddingY
        {
            get => (double)GetValue(PaddingYProperty);
            set => SetValue(PaddingYProperty, value);
        }

        public bool ShowCursor
        {
            get => (bool)GetValue(ShowCursorProperty);
            set => SetValue(ShowCursorProperty, value);
        }

        public Brush CursorBrush
        {
            get => (Brush)GetValue(CursorBrushProperty);
            set => SetValue(CursorBrushProperty, value);
        }


        private static void OnVisualPropertyChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((ChartControl)d).InvalidateVisual();

        private static void OnViewportChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ChartControl)d;

            // Unsubscribe from old viewport
            if (e.OldValue is Viewport old)
                old.PropertyChanged -= ctrl.OnViewportPropertyChanged;

            // Subscribe to new viewport so any pan/zoom triggers a redraw
            if (e.NewValue is Viewport @new)
                @new.PropertyChanged += ctrl.OnViewportPropertyChanged;

            ctrl.InvalidateVisual();
        }

        //private void Viewport_PropertyChanged(object? sender, PropertyChangedEventArgs e) =>
        //    InvalidateVisual();

        private void UpdateWidth()
        {
            if (Viewport == null) return;
            Width = Viewport.ViewportWidth * Viewport.ZoomX;
        }

        private void OnViewportPropertyChanged(
    object? sender,
    System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Only the properties that affect the axis layout need a rebuild.
            // CursorPosition changes are frequent and don't affect tick layout.
            switch (e.PropertyName)
            {
                //case nameof(Viewport.OffsetX):
                case nameof(Viewport.ZoomX):
                case nameof(Viewport.ViewportWidth):
                    //case nameof(Viewport.WorldStart):
                    //case nameof(Viewport.WorldEnd):
                    UpdateWidth();
                    break;
            }
            InvalidateVisual();
        }

        // ------------------------------------------------------------------
        // Rendering
        // ------------------------------------------------------------------

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (Viewport == null) return;

            // Clip all drawing to the control bounds
            dc.PushClip(new RectangleGeometry(
                new Rect(0, 0, ActualWidth, ActualHeight)));

            if (Series != null && Series.Values.Count >= 2)
                DrawSeries(dc);

            if (ShowCursor)
                DrawCursor(dc);

            dc.Pop(); // pop clip
        }

        private void DrawSeries(DrawingContext dc)
        {
            var values = Series!.Values;

            double maxY = values.Max(p => p.Y);
            double minY = values.Min(p => p.Y);
            double pad = PaddingY;

            var geometry = new StreamGeometry();
            using var ctx = geometry.Open();

            bool first = true;
            foreach (var pt in values)
            {
                double sx = Viewport!.WorldToScreen(pt.X);
                double sy = MapY(pt.Y, minY, maxY, pad);

                if (first)
                {
                    ctx.BeginFigure(new Point(sx, sy), isFilled: false, isClosed: false);
                    first = false;
                }
                else
                {
                    ctx.LineTo(new Point(sx, sy), isStroked: true, isSmoothJoin: false);
                }
            }

            geometry.Freeze();

            dc.DrawGeometry(
                null,
                new Pen(Series.Stroke, Series.StrokeThickness),
                geometry);
        }

        private void DrawCursor(DrawingContext dc)
        {
            if (Viewport == null) return;

            double sx = Viewport.WorldToScreen(Viewport.CursorPosition);

            // Only draw when the cursor is within the visible range
            if (sx < 0 || sx > ActualWidth) return;

            var pen = new Pen(CursorBrush, 1.0) { DashStyle = DashStyles.Dash };
            pen.Freeze();

            dc.DrawLine(pen,
                new Point(sx, 0),
                new Point(sx, ActualHeight));
        }

        // ------------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------------

        /// <summary>Maps a Y value to screen space with vertical padding so
        /// min/max points don't sit on the very edge of the control.</summary>
        private double MapY(double y, double min, double max, double pad)
        {
            double drawHeight = ActualHeight - pad * 2;
            double normalised = Math.Abs(max - min) < double.Epsilon
                ? 0.5
                : (y - min) / (max - min);

            // Flip: higher values → smaller screen Y (top of control)
            return pad + (1.0 - normalised) * drawHeight;
        }
    }
}