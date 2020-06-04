using System.Diagnostics;
using GlyphEdit.Models;

namespace GlyphEdit.Messages
{
    public class GlyphChangedEvent
    {
        public readonly GlyphFont PreviousGlyphFont;
        public readonly GlyphFont NewGlyphFont;
        public readonly int PreviousGlyphIndex;
        public readonly int NewGlyphIndex;

        [DebuggerStepThrough]
        public GlyphChangedEvent(GlyphFont previousGlyphFont, GlyphFont newNewGlyphFont, int previousGlyphIndex, int newGlyphIndex)
        {
            PreviousGlyphFont = previousGlyphFont;
            NewGlyphFont = newNewGlyphFont;
            PreviousGlyphIndex = previousGlyphIndex;
            NewGlyphIndex = newGlyphIndex;
        }
    }
}
