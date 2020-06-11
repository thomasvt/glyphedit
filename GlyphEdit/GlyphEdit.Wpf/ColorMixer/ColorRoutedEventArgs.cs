using System;
using System.Windows;

namespace GlyphEdit.Wpf.ColorMixer
{
    public class ColorRoutedEventArgs
    : RoutedEventArgs
    {
        public HslRgbColor Color { get; }

        public ColorRoutedEventArgs(RoutedEvent routedEvent, HslRgbColor color)
        : base(routedEvent)
        {
            Color = color;
        }
    }
}
