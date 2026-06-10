using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static TsTimeline.XBackgroundLayer;

namespace TsTimeline
{
    [TemplatePart(Name = "PART_SCROLL_VIEWER", Type = typeof(ScrollViewer))]
    public class CombinedTimeline : TreeView
    {
        public static readonly DependencyProperty ValueConverterProperty =
    DependencyProperty.Register(nameof(ValueConverter), typeof(IValueConverter), typeof(CombinedTimeline), new PropertyMetadata());

        public static readonly DependencyProperty TickMarginProperty =
    DependencyProperty.Register(nameof(TickMargin), typeof(double), typeof(CombinedTimeline), new FrameworkPropertyMetadata(15d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty LineIntervalProperty =
    DependencyProperty.Register(nameof(LineInterval), typeof(int), typeof(CombinedTimeline), new FrameworkPropertyMetadata(10, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ValueProperty =
    DependencyProperty.Register(nameof(Value), typeof(double), typeof(CombinedTimeline), new PropertyMetadata(valueChanged));

        public static readonly DependencyProperty MaximumProperty =
            DepProp.Register<CombinedTimeline, double>(nameof(Maximum), 1000d, FrameworkPropertyMetadataOptions.AffectsMeasure);

        public static readonly DependencyProperty MinimumProperty =
    DepProp.Register<CombinedTimeline, double>(nameof(Minimum), 0d, FrameworkPropertyMetadataOptions.AffectsMeasure);

        public static readonly DependencyProperty TrackHeightProperty =
DepProp.Register<CombinedTimeline, double>(nameof(TrackHeight), 2d);

        internal static readonly DependencyPropertyKey ScrollViewerPropertyKey =
            DepProp.RegisterReadOnly<CombinedTimeline, ScrollViewer>(nameof(ScrollViewer));

        public static readonly DependencyProperty ScrollViewerProperty = ScrollViewerPropertyKey.DependencyProperty;
        public static readonly DependencyProperty Alter0Property =
            DepProp.Register<CombinedTimeline, Brush>(nameof(Alter0), Brushes.FloralWhite);
        public static readonly DependencyProperty Alter1Property =
    DepProp.Register<CombinedTimeline, Brush>(nameof(Alter1), Brushes.WhiteSmoke);

        public static readonly DependencyProperty ViewportProperty =
    DependencyProperty.Register(nameof(Viewport), typeof(Viewport), typeof(CombinedTimeline), new PropertyMetadata());

        public static readonly DependencyProperty OffsetXProperty =
    DependencyProperty.Register(nameof(OffsetX), typeof(double), typeof(CombinedTimeline), new PropertyMetadata(0d, offsetXChanged));

        private static void offsetXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue is double offsetX)
            {
                if(d is CombinedTimeline combinedTimeline)
                {
                    combinedTimeline.ScrollViewer.Margin = new Thickness(offsetX, combinedTimeline.ScrollViewer.Margin.Top, 0, combinedTimeline.ScrollViewer.Margin.Bottom);
                    combinedTimeline.measureRenderer.Margin = new Thickness(offsetX, 0, 0, 0);
                    combinedTimeline.timeLine.Margin = new Thickness(offsetX, 0, 0, 0);
                }
            }
        }

        static CombinedTimeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CombinedTimeline), new FrameworkPropertyMetadata(typeof(CombinedTimeline)));
        }

        private MeasureRenderer? measureRenderer;
        private Timeline? timeLine;
               
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ClipsControl();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ClipsControl;
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

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }


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

        public CombinedTimeline()
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
            //LayoutUpdated += (s, e) =>
            //{
            //    Viewport.ViewportWidth = ScrollViewer.ViewportWidth;
            //};
            var axisRenderer = new CombinationLayer();
            //axisRenderer.AddLayer(new YBackgroundLayer(Brushes.FloralWhite, Brushes.WhiteSmoke));
            //axisRenderer.AddLayer(new YGridLayer());
            //axisRenderer.AddLayer(new YTickLayer());
            //axisRenderer.AddLayer(new YLabelLayer());
            axisRenderer.AddLayer(new XUpBackgroundLayer(Brushes.FloralWhite, Brushes.WhiteSmoke));
            axisRenderer.AddLayer(new XUpGridLayer());
            axisRenderer.AddLayer(new XUpTickLayer());
            axisRenderer.AddLayer(new XUpLabelLayer());
            measureRenderer.Renderers = new CombinationLayer[] { axisRenderer };

            timeLine.ValueChanged += (_, _) =>
            {
                this.Value = timeLine.Value / Viewport.ZoomX / Viewport.ScaleX;
            };

            //Viewport.PropertyChanged += (_, e) =>
            //{
            //    if (e.PropertyName == nameof(Viewport.OffsetX))
            //    {
            //        //timeLine.Value = Value * Viewport.ZoomX * Viewport.ScaleX;
            //        ScrollViewer.Margin  = new Thickness(-Viewport.OffsetX, ScrollViewer.Margin.Top, 0, ScrollViewer.Margin.Bottom);
            //    }
            //};
        }

        private static void valueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CombinedTimeline CombinedTimeline)
            {
                CombinedTimeline.timeLine.Value = CombinedTimeline.Value * CombinedTimeline.Viewport.ZoomX * CombinedTimeline.Viewport.ScaleX; ;
            }
        }
    }
}