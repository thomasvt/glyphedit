using GlyphEdit.Model;

namespace GlyphEdit.Messages.Events
{
    public class ColorPaletteChangedEvent
    {
        public ColorPalette ColorPalette { get; }

        public ColorPaletteChangedEvent(ColorPalette colorPalette)
        {
            ColorPalette = colorPalette;
        }
    }
}
