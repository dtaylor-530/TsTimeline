using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Views
{
    public class Ex
    {
        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.RegisterAttached(
                "Height",
                typeof(double),
                typeof(Ex),
                new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static void SetHeight(UIElement element, double value)
            => element.SetValue(HeightProperty, value);

        public static double GetHeight(UIElement element)
            => (double)element.GetValue(HeightProperty);

        public static readonly DependencyProperty WidthProperty =
    DependencyProperty.RegisterAttached(
        "Width",
        typeof(double),
        typeof(Ex),
        new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static void SetWidth(UIElement element, double value)
            => element.SetValue(WidthProperty, value);

        public static double GetWidth(UIElement element)
            => (double)element.GetValue(WidthProperty);

        public static readonly DependencyProperty XProperty =
            DependencyProperty.RegisterAttached(
                "X",
                typeof(double),
                typeof(Ex),
                new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static void SetX(UIElement element, double value)
            => element.SetValue(XProperty, value);

        public static double GetX(UIElement element)
            => (double)element.GetValue(XProperty);

        public static readonly DependencyProperty YProperty =
    DependencyProperty.RegisterAttached(
        "Y",
        typeof(double),
        typeof(Ex),
        new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static void SetY(UIElement element, double value)
            => element.SetValue(YProperty, value);

        public static double GetY(UIElement element)
            => (double)element.GetValue(YProperty);


        public static readonly DependencyProperty PanelTypeProperty =
DependencyProperty.RegisterAttached(
"PanelType",
typeof(PanelType),
typeof(Ex),
new FrameworkPropertyMetadata(PanelType.None, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static void SetPanelType(UIElement element, PanelType value)
            => element.SetValue(PanelTypeProperty, value);

        public static PanelType GetPanelType(UIElement element)
            => (PanelType)element.GetValue(PanelTypeProperty            );


        public static readonly DependencyProperty DirectionProperty =
    DependencyProperty.RegisterAttached(
        "Direction",
        typeof(Direction),
        typeof(Ex),
        new FrameworkPropertyMetadata(Direction.Down, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static void SetDirection(UIElement element, Direction value)
            => element.SetValue(DirectionProperty, value);

        public static Direction GetDirection(UIElement element)
            => (Direction)element.GetValue(DirectionProperty);








    }
}
