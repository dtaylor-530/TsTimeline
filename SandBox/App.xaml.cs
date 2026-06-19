using System.Windows;
using Renderers;
using TsTimeline;

namespace SandBox
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static IAxisLayer xBottomGridLayer = new XBottomGridLayer();
        static IAxisLayer xBottomTickLayer = new XBottomTickLayer();
        static IAxisLayer xBottomLabelLayer = new XBottomLabelLayer();
        static IAxisLayer yGridLayer = new YGridLayer();
        static IAxisLayer yTickLayer = new YTickLayer();
        static IAxisLayer yLabelLayer = new YLabelLayer();
  
        protected override void OnStartup(StartupEventArgs e)
        {
            var vm = new MainWindowViewModel
            {
                Player = new()
                {
                    Master = new PlayListViewModel
                    {
                        Name = "My PlayList",
                        Tracks = [],      
                    },
                    Slaves = new PlayListViewModel
                    {
                        Name = "My PlayList",
                        Tracks = [],      
                    }
                },

            Configuration = new() {
                    ChartType = ChartType.Points,
                    XAxisRenderer = new CombinationLayer(xBottomGridLayer, xBottomTickLayer, xBottomLabelLayer),
                    XAxisFactory = new X_AxisFactory(new TimelineTickGenerator()),
                    YAxisRenderer = new CombinationLayer(yGridLayer, yTickLayer, yLabelLayer),
                    YAxisFactory = new Y_AxisFactory(new YUp_TimelineTickGenerator()),
                },
                ViewportX = new() { },
                ViewportY = new() { },
                ViewportItemY = new() { End = 100, Start = 0, ViewportLength = 1000 },
                Speed = new(),
                Progress = new()
            };

            var timeSimulation = new TimeSimulationService();
            timeSimulation.Load(vm.Player, vm.Progress);
            timeSimulation.Load(vm.Speed);

            reloadData();
            vm.Configuration.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ConfigurationViewModel.ChartType))
                    reloadData();
            };

            new Window { Content = vm }.Show();
            base.OnStartup(e);

            void reloadData()
            {
                vm.Player.Master.Tracks?.Clear();
                vm.Player.Slaves.Tracks?.Clear();

                if (vm.Configuration.ChartType == TsTimeline.ChartType.Points)
                {
                    new ChartSimulationService().Load(vm.Player.Master, vm.Player.Slaves);
                    vm.ViewportY.Zoom = 2;
                    vm.ViewportX.Zoom = 10;
                    vm.Configuration.CombinedTimelineDirection = TsTimeline.Direction.Up;
                    vm.Configuration.TimelineDirection = TsTimeline.Direction.Down;
                    if (vm.Configuration.XAxisRenderer is CombinationLayer layer)
                    {
                        layer.AddLayer(xBottomLabelLayer);
                    }
                    if (vm.Configuration.YAxisRenderer is CombinationLayer ylayer)
                    {
                        ylayer.AddLayer(yLabelLayer);
                    }
                }
                else if (vm.Configuration.ChartType == TsTimeline.ChartType.Bands)
                {
                    new TrackSimulationService().Load(vm.Player.Master, vm.Player.Slaves);
                    vm.ViewportY.Zoom = 2;
                    vm.ViewportX.Zoom = 10;
                    vm.Configuration.CombinedTimelineDirection = TsTimeline.Direction.Up;
                    vm.Configuration.TimelineDirection = TsTimeline.Direction.Down;
                    if (vm.Configuration.XAxisRenderer is CombinationLayer layer)
                    {
                        layer.AddLayer(xBottomLabelLayer);
                    }
                    if (vm.Configuration.YAxisRenderer is CombinationLayer ylayer)
                    {
                        ylayer.AddLayer(yLabelLayer);
                    }
                }
                else if (vm.Configuration.ChartType == TsTimeline.ChartType.Map)
                {
                    new MapSimulationService().Load(vm.Player.Master, vm.Player.Slaves);
                    vm.ViewportY.Zoom = 0.1;
                    vm.ViewportX.Zoom = 0.1;
                    vm.ViewportItemY.End = 10000;
                    vm.ViewportX.End = 10000;
                    vm.Configuration.CombinedTimelineDirection = TsTimeline.Direction.None;
                    vm.Configuration.TimelineDirection = TsTimeline.Direction.Right;
                    if(vm.Configuration.XAxisRenderer is CombinationLayer layer)
                    {
                        layer.RemoveLayer(xBottomLabelLayer);
                    }
                    if(vm.Configuration.YAxisRenderer is CombinationLayer ylayer)
                    {
                        ylayer.RemoveLayer(yLabelLayer);
                    }
                }
                else
                {
                    throw new System.Exception(" ");
                }
            }
        }
    }
}