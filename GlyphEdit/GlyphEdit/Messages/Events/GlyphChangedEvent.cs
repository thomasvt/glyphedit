using System.Diagnostics;
using GlyphEdit.ViewModels;

namespace GlyphEdit.Messages.Events
{
    public class GlyphChangedEvent
    {
        public readonly GlyphFontViewModel PreviousGlyphFontViewModel;
        public readonly GlyphFontViewModel NewGlyphFontViewModel;
        public readonly int PreviousGlyphIndex;
        public readonly int NewGlyphIndex;

        [DebuggerStepThrough]
        public GlyphChangedEvent(GlyphFontViewModel previousGlyphFontViewModel, GlyphFontViewModel newNewGlyphFontViewModel, int previousGlyphIndex, int newGlyphIndex)
        {
            PreviousGlyphFontViewModel = previousGlyphFontViewModel;
            NewGlyphFontViewModel = newNewGlyphFontViewModel;
            PreviousGlyphIndex = previousGlyphIndex;
            NewGlyphIndex = newGlyphIndex;
        }
    }
}
