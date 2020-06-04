using System.Diagnostics;

namespace GlyphEdit.Messages.Commands
{
    public class ChangeGlyphCommand
    {
        public readonly int GlyphIndex;

        [DebuggerStepThrough]
        public ChangeGlyphCommand(int glyphIndex)
        {
            GlyphIndex = glyphIndex;
        }
    }
}
