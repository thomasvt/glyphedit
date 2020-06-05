using System.Diagnostics;
using GlyphEdit.ViewModels;

namespace GlyphEdit.Messages.Events
{
    public class GlyphFontListLoadedEvent
    {
        public readonly GlyphFontViewModel[] GlyphFonts;

        [DebuggerStepThrough]
        public GlyphFontListLoadedEvent(GlyphFontViewModel[] glyphFonts)
        {
            GlyphFonts = glyphFonts;
        }
    }
}
