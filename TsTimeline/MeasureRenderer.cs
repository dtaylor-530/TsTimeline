using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Renderers;

namespace TsTimeline
{
    public class MeasureRenderer : Control
    {
        public static readonly DependencyProperty RendererProperty =
            DependencyProperty.Register(nameof(Renderer), typeof(IAxisLayer), typeof(MeasureRenderer), new PropertyMetadata(changed));

        private static void changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is MeasureRenderer measureRenderer && e.NewValue is INotifyCollectionChanged changed)
            {
                changed.CollectionChanged += (s, e) =>
                {
                    measureRenderer.InvalidateVisual();
                };
            }
        }

        public static readonly DependencyProperty AxisFactoryProperty =
            DependencyProperty.Register(nameof(AxisFactory), typeof(IAxisFactory), typeof(MeasureRenderer), new PropertyMetadata());

        public static readonly DependencyProperty ViewportProperty =
            DependencyProperty.Register(
                nameof(Viewport),
                typeof(Viewport),
                typeof(MeasureRenderer),
                new PropertyMetadata(null, OnViewportChanged));

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(
                nameof(ItemHeight),
                typeof(double),
                typeof(MeasureRenderer),
                new PropertyMetadata(15d, OnInvalidate));

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register(
                nameof(ItemWidth),
                typeof(double),
                typeof(MeasureRenderer),
                new PropertyMetadata(15d, OnInvalidate));

        public static readonly DependencyProperty ItemCountProperty =
            DependencyProperty.Register(
                nameof(ItemCount),
                typeof(int),
                typeof(MeasureRenderer),
                new PropertyMetadata(1, OnInvalidate));

        public static readonly DependencyProperty TickMarginProperty =
            DependencyProperty.Register(
                nameof(TickMargin),
                typeof(double),
                typeof(MeasureRenderer),
                new PropertyMetadata(0d, OnInvalidate));

        public static readonly DependencyProperty Alter0Property =
            DependencyProperty.Register(
                nameof(Alter0),
                typeof(Brush),
                typeof(MeasureRenderer),
                new PropertyMetadata(Brushes.FloralWhite, OnInvalidate));

        public static readonly DependencyProperty Alter1Property =
            DependencyProperty.Register(
                nameof(Alter1),
                typeof(Brush),
                typeof(MeasureRenderer),
                new PropertyMetadata(Brushes.WhiteSmoke, OnInvalidate));

        public static readonly DependencyProperty ValueConverterProperty =
            DependencyProperty.Register(
                nameof(ValueConverter),
                typeof(IValueConverter),
                typeof(MeasureRenderer),
                new PropertyMetadata(null, OnInvalidate));

        #region properties

        public Viewport Viewport
        {
            get => (Viewport)GetValue(ViewportProperty);
            set => SetValue(ViewportProperty, value);
        }

        public double ItemHeight
        {
            get => (double)GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }
        public double ItemWidth
        {
            get => (double)GetValue(ItemWidthProperty);
            set => SetValue(ItemWidthProperty, value);
        }

        public int ItemCount
        {
            get => (int)GetValue(ItemCountProperty);
            set => SetValue(ItemCountProperty, value);
        }

        public double TickMargin
        {
            get => (double)GetValue(TickMarginProperty);
            set => SetValue(TickMarginProperty, value);
        }

        public Brush Alter0
        {
            get => (Brush)GetValue(Alter0Property);
            set => SetValue(Alter0Property, value);
        }

        public Brush Alter1
        {
            get => (Brush)GetValue(Alter1Property);
            set => SetValue(Alter1Property, value);
        }

        public IValueConverter? ValueConverter
        {
            get => (IValueConverter?)GetValue(ValueConverterProperty);
            set => SetValue(ValueConverterProperty, value);
        }

        public IAxisLayer Renderer
        {
            get { return (IAxisLayer)GetValue(RendererProperty); }
            set { SetValue(RendererProperty, value); }
        }

        public IAxisFactory AxisFactory
        {
            get { return (IAxisFactory)GetValue(AxisFactoryProperty); }
            set { SetValue(AxisFactoryProperty, value); }
        }

        #endregion  properties

        private readonly AxisLabelCache _labelCache;
        private AxisModel? _cachedModel;
        private bool _dirty = true;

        public MeasureRenderer()
        {
            _labelCache = new AxisLabelCache(new Typeface("Segoe UI"), 10, Brushes.Black);
            SizeChanged += (_, _) => MarkDirty();
        }

        private static void OnViewportChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (MeasureRenderer)d;

            if (e.OldValue is Viewport old)
                old.PropertyChanged -= self.OnViewportPropertyChanged;

            if (e.NewValue is Viewport @new)
                @new.PropertyChanged += self.OnViewportPropertyChanged;

            self.MarkDirty();
        }

        private void OnViewportPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Viewport.Offset):
                case nameof(Viewport.Zoom):
                case nameof(Viewport.ViewportLength):
                case nameof(Viewport.Start):
                case nameof(Viewport.End):
                case nameof(Viewport.MinPixelSpacing):
                    MarkDirty();
                    break;
            }
        }

        private static void OnInvalidate(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((MeasureRenderer)d).MarkDirty();

        private void MarkDirty()
        {
            _dirty = true;
            InvalidateVisual();
        }

        private void EnsureModel()
        {
            if (!_dirty && _cachedModel != null)
                return;

            _cachedModel = AxisFactory.Build(Viewport);
            _dirty = false;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            EnsureModel();

            if (_cachedModel == null /*|| _dirty == false*/)
                return;

            var formatter = ValueConverter != null
                ? (ILabelFormatter)new ConverterFormatter(ValueConverter)
                : new NumericLabelFormatter();

            var context = new AxisRenderContext
            {
                DrawingContext = drawingContext,
                Model = _cachedModel,
                Bounds = new Rect(0, 0, ActualWidth, ActualHeight),
                TickMargin = TickMargin,
                TrackHeight = ItemHeight,
                TrackWidth = ItemWidth,
                TrackCount = ItemCount,
                LabelFormatter = formatter,
                LabelCache = _labelCache,
            };
            Renderer.Render(context);
        }
    }
}