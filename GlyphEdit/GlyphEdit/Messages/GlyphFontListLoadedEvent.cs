using System.Collections.Generic;
using GlyphEdit.Model;

namespace GlyphEdit.Messages
{
    public class GlyphFontListLoadedEvent
    {
        public readonly GlyphFont[] GlyphFonts;

        public GlyphFontListLoadedEvent(GlyphFont[] glyphFonts)
        {
            GlyphFonts = glyphFonts;
        }
    }
}
