namespace Views
{

    public partial class ClipBase : TreeViewItem
    {
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


        public T? TemplateChild<T>(string name) where T : DependencyObject
        {
            return (T)this.GetTemplateChild(name);
        }

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