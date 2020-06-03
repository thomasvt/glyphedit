using GlyphEdit.Model;

namespace GlyphEdit.Messages
{
    public class GlyphChangedEvent
    {
        public readonly GlyphFont PreviousGlyphFont;
        public readonly GlyphFont NewGlyphFont;
        public readonly int PreviousGlyphIndex;
        public readonly int NewGlyphIndex;

        public GlyphChangedEvent(GlyphFont previousGlyphFont, GlyphFont newNewGlyphFont, int previousGlyphIndex, int newGlyphIndex)
        {
            PreviousGlyphFont = previousGlyphFont;
            NewGlyphFont = newNewGlyphFont;
            PreviousGlyphIndex = previousGlyphIndex;
            NewGlyphIndex = newGlyphIndex;
        }
    }
}
