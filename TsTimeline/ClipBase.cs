using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TsTimeline
{

    public partial class ClipBase : TreeViewItem, ISelectable
    {
        protected Canvas _partCanvas;

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ClipBase() 
            {   
            };
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ClipBase;
        }

        static ClipBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ClipBase),
                new FrameworkPropertyMetadata(typeof(ClipBase)));
        }

        public static readonly DependencyProperty XProperty =
DependencyProperty.Register(nameof(X), typeof(double), typeof(ClipBase), new PropertyMetadata(0d));



        public static readonly DependencyProperty YProperty =
    DependencyProperty.Register(nameof(Y), typeof(double), typeof(ClipBase), new PropertyMetadata(0d, updateY));




        private static void updateY(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClipBase clipBase && e.NewValue is double value)
            {
                //clipBase.updateY();
            }
        }

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }


        public static SelectorService SelectorService => SelectorService.Default;

        public static readonly DependencyProperty IsReadOnlyProperty =
            DepProp.Register<ClipBase, bool>(nameof(IsReadOnly));

        public PanelType PanelType
        {
            get { return (PanelType)GetValue(PanelTypeProperty); }
            set { SetValue(PanelTypeProperty, value); }
        }


        public static readonly DependencyProperty PanelTypeProperty =
            DependencyProperty.Register(nameof(PanelType), typeof(PanelType), typeof(ClipBase), new PropertyMetadata());


        public static readonly DependencyProperty DirectionProperty =
    DependencyProperty.Register(nameof(Direction), typeof(Direction), typeof(ClipBase), new PropertyMetadata(Direction.Down, changed));
        public Direction Direction
        {
            get { return (Direction)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }
        private static void changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timeline = (ClipBase)d;
            var self = (ClipBase)d;
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }


        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        private static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClipBase t)
                t.OnSelectedChanged();
        }

        protected virtual void OnSelectedChanged()
        {
            SelectorService.UpdateSelectedItems(this);
        }

        public T? TemplateChild<T>(string name) where T : DependencyObject
        {
            return (T)this.GetTemplateChild(name);
        }

        //protected void OnMouseDownSelectedChanged()
        //{
        //    SelectorService.MouseDownSelectionChanged(this);
        //}


        public static readonly RoutedEvent RenderingEvent =
              EventManager.RegisterRoutedEvent(
                  nameof(Rendering),
                  RoutingStrategy.Bubble,
                  typeof(RenderEventHandler),
                  typeof(ClipBase));

        public event RenderEventHandler Rendering
        {
            add => AddHandler(RenderingEvent, value);
            remove => RemoveHandler(RenderingEvent, value);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            RaiseEvent(new RenderEventArgs(RenderingEvent, this, dc));
        }
    }

    public sealed class RenderEventArgs : RoutedEventArgs
    {
        public RenderEventArgs(
            RoutedEvent routedEvent,
            object source,
            DrawingContext drawingContext)
            : base(routedEvent, source)
        {
            DrawingContext = drawingContext;
        }

        public DrawingContext DrawingContext { get; }
    }

    public delegate void RenderEventHandler(object sender, RenderEventArgs e);

}