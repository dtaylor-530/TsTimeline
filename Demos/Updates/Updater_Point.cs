namespace Demos
{
    public partial class Updater
    {

        void update_Point(ClipBase clipBase, Viewport viewport, UpdateType updateType)
        {
            if (clipBase.DataContext is not ViewModel viewmodel)
            {
                throw new Exception("W g fd45 fhg");
            }

            if (viewport is { Axis: Axis.X, Group: Groups.One } && viewmodel.Group == Groups.One)
            {
                if (updateType == UpdateType.Initilisation)
                {
                    //DoubleAnimation animation = new()
                    //{
                    //    From = 0,
                    //    To = X * viewport.Zoom,
                    //    Duration = TimeSpan.FromSeconds(0.5)
                    //};

                    //clipBase.BeginAnimation(Canvas.LeftProperty, animation);

                    viewmodel.WorldX = viewmodel.X * viewport.Zoom ;
                }
                else
                    viewmodel.WorldX = viewmodel.X * viewport.Zoom;

                //clipBase.Width = Width * viewport.Zoom;
            }
            else if (viewport is { Axis: Axis.Y, Group: Groups.One } && viewmodel.Group == Groups.One)
            {
                if (updateType == UpdateType.Initilisation)
                {
                    //DoubleAnimation animation = new()
                    //{
                    //    From = 0,
                    //    To = Y * viewport.Zoom,
                    //    Duration = TimeSpan.FromSeconds(0.5)
                    //};

                    //clipBase.BeginAnimation(Canvas.BottomProperty, animation);
                    viewmodel.WorldY = viewport.Length - viewmodel.Y * viewport.Zoom - viewmodel.Height;

                }
                else
                    viewmodel.WorldY = viewport.Length - viewmodel.Y * viewport.Zoom - viewmodel.Height;

                //clipBase.Height = Height * viewport.Zoom;
            }
            else if (viewport is { Axis: Axis.X, Group: Groups.Two } && viewmodel.Group == Groups.Two)
            {
                //if (updateType == UpdateType.Initilisation)
                //{
                //    DoubleAnimation animation = new()
                //    {
                //        From = 0,
                //        To = X * viewport.Zoom,
                //        Duration = TimeSpan.FromSeconds(0.5)
                //    };

                //    clipBase.BeginAnimation(Canvas.LeftProperty, animation);
                //}
                //else
                viewmodel.WorldX = viewmodel.X * viewport.Zoom;
            }
            else if (viewport is { Axis: Axis.Y, Group: Groups.Two } && viewmodel.Group == Groups.Two)
            {
                //if (updateType == UpdateType.Initilisation)
                //{
                //    DoubleAnimation animation = new()
                //    {
                //        From = 0,
                //        To = X * viewport.Zoom,
                //        Duration = TimeSpan.FromSeconds(0.5)
                //    };

                //    clipBase.BeginAnimation(Canvas.LeftProperty, animation);
                //}
                //else
                {
                    var y = App.viewportY2.MinimumSpacing * ((App.viewportY.End - viewmodel.Y) / (App.viewportY.End - App.viewportY.Start)) * viewport.Zoom - viewmodel.Height;
                    viewmodel.WorldY = y;

                    //cviewmodel.WorldHeight = App.viewportY2.MinimumSpacing * (cviewmodel.Height/ (App.viewportY.End - App.viewportY.Start)) * viewport.Zoom;
                    //        cviewmodel.WorldY = App.viewportY2.MinimumSpacing * (cviewmodel.Y / (App.viewportY.End - App.viewportY.Start)) * viewport.Zoom;
                }
            }
            //if (viewport is { Axis: Axis.Y, Key: "Y2" } )
            //{
            //    Canvas.SetTop(clipBase, viewport.Length - Y * viewport.Zoom);
            //}
            return;
        }
    }
}
