using System.Windows;

namespace TsTimeline
{
    public interface IUpdater
    {
        bool CanUpdate(object control);
        void Update(FrameworkElement control, object context);
        //void UpdateY(ClipBase clipBase);
    }
}