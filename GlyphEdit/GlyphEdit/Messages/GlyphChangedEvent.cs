using GlyphEdit.Model;

namespace GlyphEdit.Messages
{
    public class GlyphChangedEvent
    {
        public readonly GlyphFont GlyphFont;
        public readonly int GlyphIndex;

        public GlyphChangedEvent(GlyphFont glyphFont, int glyphIndex)
        {
            GlyphFont = glyphFont;
            GlyphIndex = glyphIndex;
        }
    }
}
