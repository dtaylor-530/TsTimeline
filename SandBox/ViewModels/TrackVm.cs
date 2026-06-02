using System.Collections.ObjectModel;
using System.Linq;
using TsTimeline;

namespace SandBox
{
    public class TrackVm : Notification
    {
        public string Name => "Test Track";

        public double Min
        {
            get => Clips.OfType<HoldClipVm>().Min(x => x.StartValue);
        }
        
        public double Max
        {
            get => Clips.OfType<HoldClipVm>().Max(x => x.EndValue);
        }
        
        public ObservableCollection<Notification> Clips { get; } = new ObservableCollection<Notification>();
    }
}