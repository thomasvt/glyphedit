using GlyphEdit.Model;

namespace GlyphEdit.Messages
{
    public class GlyphFontChangedEvent
    {
        public readonly GlyphFont GlyphFont;

        public GlyphFontChangedEvent(GlyphFont glyphFont)
        {
            GlyphFont = glyphFont;
        }
    }
}
