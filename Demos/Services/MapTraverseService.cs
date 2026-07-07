namespace Demos
{
    public class MapTraverseService : Notification
    {
        private List<ViewModel> lines = [];
        List<Viewport> viewports = [];
        private Viewport xViewport, yViewport;
        private double xZoom, yZoom;
        private ViewModel playlist;

        public MapTraverseService()
        {
        }

        public void Load(Viewport viewport)
        {
            viewports.Add(viewport);
            if (viewport.Axis == Axis.X)
            {
                xViewport = viewport;
                xZoom = xViewport.Zoom;
            }
            else if (viewport.Axis == Axis.Y)
            {
                yViewport = viewport;
                yZoom = yViewport.Zoom;
            }
        }

        public void Load(TrackService trackService)
        {

            bool initialised = false;
            double maxY = 0, maxX = 0;

            trackService.Subscribe(a =>
            {
                var (index, progress) = a;

                initialise();
                int countriesCount = playlist.Children.Count();
                var from = countries(index);
                var to = countries(Math.Min(index + 1, countriesCount - 1));
                double fromCenterX = from.Left.Value + from.Width.Value / 2;
                double toCenterX = to.Left.Value + to.Width.Value / 2;
                double fromCenterY = from.Top.Value + from.Height.Value / 2;
                double toCenterY = to.Top.Value + to.Height.Value / 2;
                double x = MathHelpers.Lerp(fromCenterX, toCenterX, progress);
                double y = MathHelpers.Lerp(fromCenterY, toCenterY, progress);

                double zoom = 1.0 - 0.2 * Math.Sin(progress * Math.PI);

                //xViewport.Zoom = xZoom * zoom;
                //yViewport.Zoom = yZoom * zoom;

                xViewport.Offset = x * xViewport.Zoom;
                yViewport.Offset = y * yViewport.Zoom;

                foreach (var line in lines)
                {
                    if (line.Axis == Axis.X)
                        if (xViewport.Offset < xViewport.Length / 2)
                            line.WorldX = xViewport.Offset;
                        else if (xViewport.Offset > maxX - xViewport.Length / 2)
                            line.WorldX = xViewport.Offset - maxX + xViewport.Length / 2;
                        else
                            line.WorldX = xViewport.Length / 2;

                    else if (line.Axis == Axis.Y)
                        if (yViewport.Offset < yViewport.Length / 2)
                            line.WorldY = yViewport.Offset;
                        else if (yViewport.Offset > maxY - yViewport.Length / 2)
                            line.WorldY = yViewport.Offset - maxY + yViewport.Length / 2;
                        else
                            line.WorldY = yViewport.Length / 2;
                }
            });


            void initialise()
            {
                if (initialised == false)
                {
                    initialised = true;
                    foreach (Country child in playlist.Children)
                    {
                        maxX = Math.Max(maxX, child.Left.Value + child.Width.Value);
                        maxY = Math.Max(maxY, child.Top.Value + child.Height.Value);
                    }
                }
            }

            Country countries(int i)
            {
                return (Country)playlist.Children.ElementAt(i);
            }
        }
    
        

        public void Load(ViewModel viewModel)
        {
            if (viewModel.Key == Keys.Playlist)
            {
                this.playlist = viewModel;
            }
            else if (viewModel.Key == Keys.Line)
            {
                loadLine(viewModel);
                return;
            }
            else
                throw new Exception("R Sr");

        


            void loadLine(ViewModel playList)
            {
                this.lines.Add(playList);
            }
        }
    }
}
