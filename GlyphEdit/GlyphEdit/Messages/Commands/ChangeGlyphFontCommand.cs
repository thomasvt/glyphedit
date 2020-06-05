using System.Diagnostics;
using GlyphEdit.ViewModels;

namespace GlyphEdit.Messages.Commands
{
    public class ChangeGlyphFontCommand
    {
        public readonly GlyphFontViewModel GlyphFontViewModel;

        [DebuggerStepThrough]
        public ChangeGlyphFontCommand(GlyphFontViewModel glyphFontViewModel)
        {
            GlyphFontViewModel = glyphFontViewModel;
        }
    }
}
