using GlyphEdit.Model;

namespace GlyphEdit.Messages.Events
{
    public class ColorPaletteListLoadedEvent
    {
        public readonly ColorPalette[] Palettes;

        public ColorPaletteListLoadedEvent(ColorPalette[] palettes)
        {
            Palettes = palettes;
        }
    }
}
