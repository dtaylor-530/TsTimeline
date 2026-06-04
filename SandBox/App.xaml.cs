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
                Player = new(),
                Viewport = new() { },
                Speed = new(),
                Chart = new(),
                Progress = new()
            };

            var timeSimulation = new TimeSimulationService();
            timeSimulation.Load(vm.Player,vm.Progress, vm.Viewport);
            timeSimulation.Load(vm.Speed);
            new ChartSimulationService().Load(vm.Chart);
            vm.Chart.Viewport = vm.Viewport;
            new TrackSimulationService().Load(vm.Player);
            new Window { Content = vm }.Show();
            base.OnStartup(e);
        }
    }
}