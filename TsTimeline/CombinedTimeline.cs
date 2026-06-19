using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Renderers;

namespace TsTimeline
{
    [TemplatePart(Name = "PART_SCROLL_VIEWER", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_ITEMS_PRESENTER", Type = typeof(ItemsPresenter))]
    public class CombinedTimeline : TreeView
    {
        public static readonly DependencyProperty ValueConverterProperty =
            DependencyProperty.Register(nameof(ValueConverter), typeof(IValueConverter), typeof(CombinedTimeline), new PropertyMetadata());

        public static readonly DependencyProperty TickMarginProperty =
            DependencyProperty.Register(nameof(TickMargin), typeof(double), typeof(CombinedTimeline), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty LineIntervalProperty =
            DependencyProperty.Register(nameof(LineInterval), typeof(int), typeof(CombinedTimeline), new FrameworkPropertyMetadata(10, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ValueProperty =
    DependencyProperty.Register(nameof(Value), typeof(double), typeof(CombinedTimeline), new PropertyMetadata(valueChanged));

        public static readonly DependencyProperty TrackHeightProperty =
            DepProp.Register<CombinedTimeline, double>(nameof(TrackHeight), 2d);

        public static readonly DependencyProperty Alter0Property =
            DepProp.Register<CombinedTimeline, Brush>(nameof(Alter0), Brushes.FloralWhite);

        public static readonly DependencyProperty Alter1Property =
            DepProp.Register<CombinedTimeline, Brush>(nameof(Alter1), Brushes.WhiteSmoke);

        public static readonly DependencyProperty ViewportXProperty =
            DependencyProperty.Register(nameof(ViewportX), typeof(Viewport), typeof(CombinedTimeline), new PropertyMetadata(_changedX));

        public static readonly DependencyProperty ViewportYProperty =
            DependencyProperty.Register(nameof(ViewportY), typeof(Viewport), typeof(CombinedTimeline), new PropertyMetadata(_changedY));


        public static readonly DependencyProperty OffsetXProperty =
            DependencyProperty.Register(nameof(OffsetX), typeof(double), typeof(CombinedTimeline), new PropertyMetadata(0d, offsetXChanged));

        public static readonly DependencyProperty PanelTypeProperty =
            DependencyProperty.Register(nameof(PanelType), typeof(PanelType), typeof(CombinedTimeline), new PropertyMetadata(chartTypeChanged));


        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(nameof(Direction), typeof(Direction), typeof(CombinedTimeline), new PropertyMetadata(changed));


        public static readonly DependencyProperty XRendererProperty =
        DependencyProperty.Register(nameof(XRenderer), typeof(IAxisLayer), typeof(CombinedTimeline), new PropertyMetadata(changedX));

        private static void changedX(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CombinedTimeline combined)
                if (combined.x_measureRenderer != null)
                {
                    if (e.NewValue is IAxisFactory axisFactory)
                        combined.x_measureRenderer.AxisFactory = axisFactory;
                    else if (e.NewValue is IAxisLayer axisLayer)
                        combined.x_measureRenderer.Renderer = axisLayer;
                }
        }

        public static readonly DependencyProperty XAxisFactoryProperty =
        DependencyProperty.Register(nameof(XAxisFactory), typeof(IAxisFactory), typeof(CombinedTimeline), new PropertyMetadata(changedX));

        public static readonly DependencyProperty YRendererProperty =
        DependencyProperty.Register(nameof(YRenderer), typeof(IAxisLayer), typeof(CombinedTimeline), new PropertyMetadata(changedY));

        private static void changedY(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CombinedTimeline combined)
                if (combined.y_measureRenderer != null)
                {
                    if (e.NewValue is IAxisFactory axisFactory)
                        combined.y_measureRenderer.AxisFactory = axisFactory;
                    else if (e.NewValue is IAxisLayer axisLayer)
                        combined.y_measureRenderer.Renderer = axisLayer;
                }
        }

        public static readonly DependencyProperty YAxisFactoryProperty =
        DependencyProperty.Register(nameof(YAxisFactory), typeof(IAxisFactory), typeof(CombinedTimeline), new PropertyMetadata(changedY));

        private static void changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void _changedX(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        private static void _changedY(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void chartTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is TreeView treeView)
            //    treeView.InvalidateVisual();
        }

        private static void offsetXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is double offsetX)
            {
                if (d is CombinedTimeline combinedTimeline)
                {
                    if (combinedTimeline.scrollViewer is { } scrollViewer)
                    {
                        scrollViewer.Margin = new Thickness(offsetX, scrollViewer.Margin.Top, 0, scrollViewer.Margin.Bottom);
                    }
                    else
                    {
                        combinedTimeline.itemsPresenter.Margin = new Thickness(offsetX, combinedTimeline.itemsPresenter.Margin.Top, 0, combinedTimeline.itemsPresenter.Margin.Bottom);
                    }

                    if (combinedTimeline.x_measureRenderer != null)
                        combinedTimeline.x_measureRenderer.Margin = new Thickness(offsetX, 0, 0, 0);

                    if (combinedTimeline.y_measureRenderer != null)
                        combinedTimeline.y_measureRenderer.Margin = new Thickness(offsetX, 0, 0, 0);

                    if (combinedTimeline.timeLine != null)
                        combinedTimeline.timeLine.Margin = new Thickness(offsetX, 0, 0, 0);
                }
            }
        }

        static CombinedTimeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CombinedTimeline), new FrameworkPropertyMetadata(typeof(CombinedTimeline)));
        }

        private ScrollViewer? scrollViewer;
        private MeasureRenderer? x_measureRenderer, y_measureRenderer;
        private Timeline? timeLine;
        private ItemsPresenter? itemsPresenter;

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ClipBase();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ClipBase;
        }

        #region Properties

        public IAxisLayer XRenderer
        {
            get { return (IAxisLayer)GetValue(XRendererProperty); }
            set { SetValue(XRendererProperty, value); }
        }

        public IAxisFactory XAxisFactory
        {
            get { return (IAxisFactory)GetValue(XAxisFactoryProperty); }
            set { SetValue(XAxisFactoryProperty, value); }
        }
        public IAxisLayer YRenderer
        {
            get { return (IAxisLayer)GetValue(YRendererProperty); }
            set { SetValue(YRendererProperty, value); }
        }

        public IAxisFactory YAxisFactory
        {
            get { return (IAxisFactory)GetValue(YAxisFactoryProperty); }
            set { SetValue(YAxisFactoryProperty, value); }
        }

        public Direction Direction
        {
            get { return (Direction)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public PanelType PanelType
        {
            get { return (PanelType)GetValue(PanelTypeProperty); }
            set { SetValue(PanelTypeProperty, value); }
        }

        public Viewport ViewportX
        {
            get { return (Viewport)GetValue(ViewportXProperty); }
            set { SetValue(ViewportXProperty, value); }
        }

        public Viewport ViewportY
        {
            get { return (Viewport)GetValue(ViewportYProperty); }
            set { SetValue(ViewportYProperty, value); }
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

                    var ss = ViewportX.Zoom * delta;

                    ViewportX.Zoom = Math.Min(Math.Max(0.125f, ss), 32.0f);
                    e.Handled = true;
                }
            };

            PreviewMouseMove += (s, e) => { InvalidateVisual(); };
            MouseLeave += (s, e) => { InvalidateVisual(); };
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            scrollViewer = GetTemplateChild("PART_SCROLL_VIEWER") as ScrollViewer;
            x_measureRenderer = GetTemplateChild("PART_MEASURE_RENDERER") as MeasureRenderer;
            y_measureRenderer = GetTemplateChild("PART_Y_MEASURE_RENDERER") as MeasureRenderer;
            timeLine = GetTemplateChild("PART_TIMELINE") as Timeline;
            itemsPresenter = GetTemplateChild("PART_ITEMS_PRESENTER") as ItemsPresenter;

            LayoutUpdated += (s, e) =>
            {
                ViewportY.ViewportLength = scrollViewer?.ViewportHeight ?? ViewportY.ViewportLength;
            };

            if (x_measureRenderer != null)
            {
                x_measureRenderer.Renderer = XRenderer;
                x_measureRenderer.AxisFactory = XAxisFactory;
            }
            if (y_measureRenderer != null)
            {
                y_measureRenderer.Renderer = YRenderer;
                y_measureRenderer.AxisFactory = YAxisFactory;
            }

            timeLine.ValueChanged += (_, _) =>
            {
                this.Value = timeLine.Value / ViewportX.Zoom / ViewportX.Scale;
            };


            scrollViewer?.ScrollChanged += (s, e) =>
            {
                ViewportY.Offset = -scrollViewer.VerticalOffset;
            };

            ViewportX.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(Viewport.Offset))
                {
                    scrollViewer?.ScrollToHorizontalOffset(ViewportX.Offset);
                }
            };
        }

        private static void valueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CombinedTimeline CombinedTimeline)
            {
                CombinedTimeline.timeLine.Value = CombinedTimeline.Value * CombinedTimeline.ViewportX.Zoom * CombinedTimeline.ViewportX.Scale; ;
            }
        }
    }
}