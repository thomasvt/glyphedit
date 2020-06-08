using System.Windows;

namespace GlyphEdit.Wpf.ColorGrid
{
    public class ColorPatchRoutedEventArgs
    : RoutedEventArgs
    {
        public ColorPatch ColorPatch { get; }

        public ColorPatchRoutedEventArgs(RoutedEvent routedEvent, ColorPatch colorPatch)
        : base(routedEvent)
        {
            ColorPatch = colorPatch;
        }
    }
}
