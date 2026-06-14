using System.Windows;

namespace SandBox
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            var vm = new MainWindowViewModel
            {
                Player = new()
                {
                    PlayList = new PlayListViewModel
                    {
                        Name = "My PlayList",
                        Tracks = [],
                        Stacks = []
                    }
                },
                Configuration = new() { ChartType = TsTimeline.ChartType.Points },
                Viewport = new() { },
                Speed = new(),
                Progress = new()
            };

            var timeSimulation = new TimeSimulationService();
            timeSimulation.Load(vm.Player, vm.Progress, vm.Viewport);
            timeSimulation.Load(vm.Speed);

            reloadData();
            vm.Configuration.PropertyChanged += (s,e)=> reloadData();

            new Window { Content = vm }.Show();
            base.OnStartup(e);

            void reloadData()
            {
                vm.Player.PlayList.Stacks?.Clear();
                vm.Player.PlayList.Tracks?.Clear();

                if (vm.Configuration.ChartType == TsTimeline.ChartType.Points)
                {
                    new ChartSimulationService().Load(vm.Player.PlayList);
                }
                else if (vm.Configuration.ChartType == TsTimeline.ChartType.Bands)
                {
                    new TrackSimulationService().Load(vm.Player.PlayList);
                }
                else
                {
                    throw new System.Exception(" ");
                }
            }
        }
    }
}