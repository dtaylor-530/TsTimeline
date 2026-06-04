using System.Collections.ObjectModel;
using System.Linq;
using TsTimeline;

namespace SandBox
{
    public class TrackViewModel : Notification
    {
        public string Name { get; set; }= "Test Track";

        public double Min
        {
            get => Clips.OfType<HoldClipViewModel>().Min(x => x.StartValue);
        }
        
        public double Max
        {
            get => Clips.OfType<HoldClipViewModel>().Max(x => x.EndValue);
        }
        
        public ObservableCollection<Notification> Clips { get; set; }
    }
}