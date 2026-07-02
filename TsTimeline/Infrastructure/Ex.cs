using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace TsTimeline
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
    }
}
