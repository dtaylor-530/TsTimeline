namespace SandBox
{
    public partial class  ViewModel
    {       

        void update_Point(ClipBase clipBase, Viewport viewport, UpdateType updateType)
        {
            
            if (viewport is { Axis: Axis.X, Group: Groups.One } && this.Group == Groups.One)
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
   
                    Canvas.SetLeft(clipBase, X * viewport.Zoom);
                }
                else
                    Canvas.SetLeft(clipBase, X * viewport.Zoom);

                //clipBase.Width = Width * viewport.Zoom;
            }
            else if (viewport is { Axis: Axis.Y, Group: Groups.One } && this.Group == Groups.One)
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
                    Canvas.SetBottom(clipBase, Y * viewport.Zoom);

                }
                else
                    Canvas.SetBottom(clipBase, Y * viewport.Zoom);

                //clipBase.Height = Height * viewport.Zoom;
            }
            else if (viewport is { Axis: Axis.X, Group: Groups.Two } && this.Group == Groups.Two)
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
                    Canvas.SetLeft(clipBase, X * viewport.Zoom);
            }
            else if (viewport is { Axis: Axis.Y, Group: Groups.Two } && this.Group == Groups.Two)
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
                    var y = App.viewportY2.MinimumSpacing * (Y / (App.viewportY.End - App.viewportY.Start)) * viewport.Zoom; ;
                    Canvas.SetBottom(clipBase, y);

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
