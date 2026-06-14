using System.Collections.ObjectModel;

namespace SandBox
{
    public class PlayListViewModel 
    {
        public string Name { get; set; } 

        public ObservableCollection<Notification> Tracks { get; set; } 
        public ObservableCollection<Notification> Stacks { get; set; } 
    }
}
