namespace Demos
{
    public class GridHighlightService : Notification
    {

        private ViewModel playlist;

        public GridHighlightService()
        {
        }

        public void Load(ViewModel viewModel)
        {
            if (viewModel.Key == Keys.Area)
            {
                TimeService.Instance.Subscribe(progress =>
                {
                    double fractional = progress - Math.Truncate(progress);
                    var i = (int)Math.Round(progress - 0.5);
                    double t = MathHelpers.SmootherStep(progress - i);
                    //var child = (Country)playlist.Children.ElementAt(i);
                    int countriesCount = playlist.Children.Count();
                    var from = countries(i);
                    var to = countries(Math.Min(i + 1, countriesCount - 1));
                    double fromCenterX = from.Left.Value + from.Width.Value / 2;
                    double toCenterX = to.Left.Value + to.Width.Value / 2;
                    double fromCenterY = from.Top.Value + from.Height.Value / 2;
                    double toCenterY = to.Top.Value + to.Height.Value / 2;
                    double width = MathHelpers.Lerp(from.Position.Left, to.Position.Left, t);
                    double height = MathHelpers.Lerp(from.Position.Top, to.Position.Top, t);
                    double x = MathHelpers.Lerp(from.Position.Right, to.Position.Right, t);
                    double y = MathHelpers.Lerp(from.Position.Bottom, to.Position.Bottom, t);


                    if (viewModel.Axis == Axis.X && viewModel.Order == 1)
                    {
                        viewModel.WorldWidth = width;
                        viewModel.WorldHeight = (viewModel.Parent as ViewModel).Position.Height - 16;
                    }
                    else if (viewModel.Axis == Axis.Y && viewModel.Order == 1)
                    {
                        viewModel.WorldHeight = height;
                        viewModel.WorldWidth = (viewModel.Parent as ViewModel).Position.Width - 16;
                    }
                    else if (viewModel.Axis == Axis.X && viewModel.Order == 2)
                    {
                        viewModel.WorldX = x;
                        viewModel.WorldWidth = (viewModel.Parent as ViewModel).Position.Width - 16 - x;
                        viewModel.WorldHeight = (viewModel.Parent as ViewModel).Position.Height;
                    }
                    else if (viewModel.Axis == Axis.Y && viewModel.Order == 2)
                    {
                        viewModel.WorldY = y;
                        viewModel.WorldHeight = (viewModel.Parent as ViewModel).Position.Height - y;
                        viewModel.WorldWidth = (viewModel.Parent as ViewModel).Position.Width - 16;
                    }
                });
            }
            else if (viewModel.Key == Keys.Playlist)
            {
                playlist = viewModel;
                return;
            }
            else
                throw new Exception("R Sr");

         
            Country countries(int i)
            {
                return (Country)playlist.Children.ElementAt(i);
            }
        }
    }
}
