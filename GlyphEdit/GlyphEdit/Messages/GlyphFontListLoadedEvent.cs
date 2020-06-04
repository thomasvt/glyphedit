using System.Collections.Generic;
using System.Diagnostics;
using GlyphEdit.Models;

namespace GlyphEdit.Messages
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
