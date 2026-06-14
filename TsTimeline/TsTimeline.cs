using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Renderers;

namespace TsTimeline
{
    [TemplatePart(Name = "PART_SCROLL_VIEWER", Type = typeof(ScrollViewer))]
    public class TsTimeline : TreeView
    {
        public static readonly DependencyProperty ValueConverterProperty =
    DependencyProperty.Register(nameof(ValueConverter), typeof(IValueConverter), typeof(TsTimeline), new PropertyMetadata());

        public static readonly DependencyProperty TickMarginProperty =
    DependencyProperty.Register(nameof(TickMargin), typeof(double), typeof(TsTimeline), new FrameworkPropertyMetadata(15d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty LineIntervalProperty =
    DependencyProperty.Register(nameof(LineInterval), typeof(int), typeof(TsTimeline), new FrameworkPropertyMetadata(10, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ValueProperty =
    DependencyProperty.Register(nameof(Value), typeof(double), typeof(TsTimeline), new PropertyMetadata(valueChanged));

    //    public static readonly DependencyProperty MaximumProperty =
    //        DepProp.Register<TsTimeline, double>(nameof(Maximum), 1000d, FrameworkPropertyMetadataOptions.AffectsMeasure);

    //    public static readonly DependencyProperty MinimumProperty =
    //DepProp.Register<TsTimeline, double>(nameof(Minimum), 0d, FrameworkPropertyMetadataOptions.AffectsMeasure);

        public static readonly DependencyProperty TrackHeightProperty =
DepProp.Register<TsTimeline, double>(nameof(TrackHeight), 15d);

        internal static readonly DependencyPropertyKey ScrollViewerPropertyKey =
            DepProp.RegisterReadOnly<TsTimeline, ScrollViewer>(nameof(ScrollViewer));

        public static readonly DependencyProperty ScrollViewerProperty = ScrollViewerPropertyKey.DependencyProperty;
        public static readonly DependencyProperty Alter0Property =
            DepProp.Register<TsTimeline, Brush>(nameof(Alter0), Brushes.FloralWhite);
        public static readonly DependencyProperty Alter1Property =
    DepProp.Register<TsTimeline, Brush>(nameof(Alter1), Brushes.WhiteSmoke);

        public static readonly DependencyProperty ViewportProperty =
    DependencyProperty.Register(nameof(Viewport), typeof(Viewport), typeof(TsTimeline), new PropertyMetadata());

        public static readonly DependencyProperty OffsetXProperty =
    DependencyProperty.Register(nameof(OffsetX), typeof(double), typeof(TsTimeline), new PropertyMetadata(0d));

        private MeasureRenderer? measureRenderer;
        private Timeline? timeLine;
        private ItemsPresenter? itemsPresenter;

        static TsTimeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TsTimeline), new FrameworkPropertyMetadata(typeof(TsTimeline)));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ClipBase();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ClipBase;
        }

        #region Properties
        public Viewport Viewport
        {
            get { return (Viewport)GetValue(ViewportProperty); }
            set { SetValue(ViewportProperty, value); }
        }

        public IValueConverter ValueConverter
        {
            get { return (IValueConverter)GetValue(ValueConverterProperty); }
            set { SetValue(ValueConverterProperty, value); }
        }

        public double TickMargin
        {
            get { return (double)GetValue(TickMarginProperty); }
            set { SetValue(TickMarginProperty, value); }
        }

        public int LineInterval
        {
            get { return (int)GetValue(LineIntervalProperty); }
            set { SetValue(LineIntervalProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        //public double Maximum
        //{
        //    get => (double)GetValue(MaximumProperty);
        //    set => SetValue(MaximumProperty, value);
        //}

        //public double Minimum
        //{
        //    get => (double)GetValue(MinimumProperty);
        //    set => SetValue(MinimumProperty, value);
        //}


        public double TrackHeight
        {
            get => (double)GetValue(TrackHeightProperty);
            set => SetValue(TrackHeightProperty, value);
        }

        public ScrollViewer ScrollViewer
        {
            get { return (ScrollViewer)GetValue(ScrollViewerProperty); }
            private set => SetValue(ScrollViewerPropertyKey, value);
        }
        
        public Brush Alter0
        {
            get { return (Brush)GetValue(Alter0Property); }
            set { SetValue(Alter0Property, value); }
        }
        public Brush Alter1
        {
            get { return (Brush)GetValue(Alter1Property); }
            set { SetValue(Alter1Property, value); }
        }

        public double OffsetX
        {
            get { return (double)GetValue(OffsetXProperty); }
            set { SetValue(OffsetXProperty, value); }
        }


        #endregion Properties
        public TsTimeline()
        {

            PreviewMouseWheel += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    var delta = 1f + e.Delta * 0.001f;

                    var ss = Viewport.ZoomX * delta;

                    Viewport.ZoomX = Math.Min(Math.Max(0.125f, ss), 32.0f);
                    e.Handled = true;
                }
            };

            PreviewMouseMove += (s, e) => { InvalidateVisual(); };
            MouseLeave += (s, e) => { InvalidateVisual(); };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ScrollViewer = GetTemplateChild("PART_SCROLL_VIEWER") as ScrollViewer;
            measureRenderer = GetTemplateChild("PART_MEASURE_RENDERER") as MeasureRenderer;
            timeLine = GetTemplateChild("PART_TIMELINE") as Timeline;
            itemsPresenter = GetTemplateChild("PART_ITEMSPRESENTER") as ItemsPresenter;

            LayoutUpdated += (s, e) =>
            {
                if(ScrollViewer is { } scrollViewer)
                {
                    Viewport.ViewportWidth = scrollViewer.ViewportWidth;
                    Viewport.ViewportHeight = scrollViewer.ViewportHeight;
                    Point position = scrollViewer.TranslatePoint(new Point(0, 0), this);
                    OffsetX = position.X;
                    Viewport.OffsetX = scrollViewer.HorizontalOffset;
                    //ScrollViewer.Width = Viewport.ViewportWidth;
                }
            };

            ScrollViewer.ScrollChanged += (s, e) =>
            {
                Viewport.OffsetX = ScrollViewer.HorizontalOffset;
            };

            timeLine.ValueChanged += (_, _) =>
            {
                this.Value = timeLine.Value / Viewport.ZoomX / Viewport.ScaleX;
            };
            var axisRenderer = new CombinationLayer(
                new XBackgroundLayer(Brushes.FloralWhite, Brushes.WhiteSmoke),
                new XGridLayer(),
                new XTickLayer(),
                new XLabelLayer());
            measureRenderer.Renderer = axisRenderer;
            measureRenderer.AxisFactory = new X_AxisFactory(new TimelineTickGenerator());
        }
        private static void valueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TsTimeline tsTimeLine)
            {
                tsTimeLine.timeLine.Value = tsTimeLine.Value * tsTimeLine.Viewport.ZoomX * tsTimeLine.Viewport.ScaleX; ;
            }
        }
    }
}