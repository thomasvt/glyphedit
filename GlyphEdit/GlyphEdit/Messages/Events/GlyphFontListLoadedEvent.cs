using System.Diagnostics;
using GlyphEdit.Models;

namespace GlyphEdit.Messages.Events
{
    public class GlyphFontListLoadedEvent
    {
        public readonly GlyphFont[] GlyphFonts;

        [DebuggerStepThrough]
        public GlyphFontListLoadedEvent(GlyphFont[] glyphFonts)
        {
            GlyphFonts = glyphFonts;
        }
    }
}
