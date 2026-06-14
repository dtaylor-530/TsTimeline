using System.Collections.ObjectModel;
using System.Linq;

namespace SandBox
{
    public class TrackViewModel : Notification
    {
        public string Name { get; set; } = "Test Track";

        public int Order { get; set; }

        public ObservableCollection<Notification> Clips { get; set; }

        public override string ToString()
        {
            return $"{min}/{max}";

            double min() => Clips.OfType<HoldClipViewModel>().Min(x => x.StartValue);
            double max() => Clips.OfType<HoldClipViewModel>().Max(x => x.EndValue);
        }
    }
}