namespace SandBox
{
    public partial class ViewModel
    {
        bool initialise_Playlist(ClipBase clipBase, Viewport viewport)
        {
            if (clipBase.DataContext is not ViewModel{ Group: Groups.One})
                return false;
            if (viewport.Group != Groups.One)
                return false;
            var scrollViewer = clipBase.TemplateChild<ScrollViewer>("PART_SCROLL_VIEWER");

            viewport.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Viewport.Offset))
                    //if (viewport.Axis == Axis.X)
                    //    scrollViewer.ScrollToHorizontalOffset(viewport.Offset);
                    //else if (viewport.Axis == Axis.Y)
                    //    scrollViewer.ScrollToHorizontalOffset(viewport.Offset);
                    if (viewport.Axis == Axis.X)
                    {
                        //double centerX = country.Left + country.Width / 2;
                        //double centerY = country.Top + country.Height / 2;

                        double x = viewport.Offset - scrollViewer.ViewportWidth / 2;
 
                        x = Math.Max(0, Math.Min(x, scrollViewer.ExtentWidth - scrollViewer.ViewportWidth));
                        //y = Math.Max(0, Math.Min(y, scrollViewer.ExtentHeight - scrollViewer.ViewportHeight));

                        scrollViewer.ScrollToHorizontalOffset(x);
                        //scrollViewer.ScrollToVerticalOffset(y);
                    }
                    else if (viewport.Axis == Axis.Y)
                    {
                        //double centerY = country.Top + country.Height / 2;
                        double y = viewport.Offset - scrollViewer.ViewportHeight / 2;
                        y = Math.Max(0, Math.Min(y, scrollViewer.ExtentHeight - scrollViewer.ViewportHeight));
                        scrollViewer.ScrollToVerticalOffset(y);
                    }

            };
            return true;
        }

    }
}
