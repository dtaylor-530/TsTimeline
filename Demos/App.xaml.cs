namespace Demos
{

    public partial class App : Application
    {
        public static IEnumerable ChartTypes => Enum.GetValues<ChartType>();
        private ClipBaseTree treeView;
        private CustomStyleSelector styleSelector;

        protected override void OnStartup(StartupEventArgs e)
        {
            treeView = new ClipBaseTree();
            initialiseViewPorts();
            //initialiseLayout();
            initialise();

            TimeService.Instance.Load(speed);
            TimeService.Instance.Load(MediaService);
            mapSimulationService.Load(viewportX);
            mapSimulationService.Load(viewportY);

            chartTypeViewModel.Enum = ChartType.Points;
            styleSelector = App.Current.Resources["CustomStyleSelector"] as CustomStyleSelector;
            var templateSelector = App.Current.Resources["ClipTemplateSelector"] as DataTemplateSelector;
            treeView.ItemsSource = new[] { viewmodel };
            treeView.ItemContainerStyleSelector = styleSelector;
            treeView.ItemTemplateSelector = templateSelector;

            var window = new Window
            {
                Content = treeView
            };

            reloadData();

            chartTypeViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ViewModel.Enum))
                    reloadData();
            };


            window.Show();

            base.OnStartup(e);

        }

        public static Brush CountryBrush
        {
            get;

        } = new DrawingBrush(Drawing());

        static Drawing Drawing()
        {
            var group = new DrawingGroup();

            using (var drawingcontext = group.Open())
            {
                CountryTextureLayer.Draw(drawingcontext);
            }

            return group;
        }
    }

    public record Context(FrameworkElement Element, Viewport Viewport, UpdateType UpdateType);
    public record InitialisationContext(FrameworkElement Element, Viewport Viewport) : Context(Element, Viewport, UpdateType.Initilisation);
    public record UpdateContext(FrameworkElement Element, Viewport Viewport) : Context(Element, Viewport, UpdateType.Viewport);
    public record RenderContext(FrameworkElement Element, Viewport Viewport, DrawingContext DrawingContext, ViewModel Playlist) : Context(Element, Viewport, UpdateType.Render)
    {
    }

    public enum UpdateType
    {
        Initilisation,
        Viewport,
        Vector,
        Render
    }

}
