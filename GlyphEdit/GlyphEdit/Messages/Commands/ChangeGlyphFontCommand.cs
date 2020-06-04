using System.Diagnostics;
using GlyphEdit.Models;

namespace GlyphEdit.Messages.Commands
{
    public class ChangeGlyphFontCommand
    {
        public readonly GlyphFont GlyphFont;

        [DebuggerStepThrough]
        public ChangeGlyphFontCommand(GlyphFont glyphFont)
        {
            GlyphFont = glyphFont;
        }
    }
}
