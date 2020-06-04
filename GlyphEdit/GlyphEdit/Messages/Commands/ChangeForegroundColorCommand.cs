using GlyphEdit.Models;

namespace GlyphEdit.Messages.Commands
{
    public class ChangeForegroundColorCommand
    {
        public readonly GlyphColor Color;

        public ChangeForegroundColorCommand(GlyphColor color)
        {
            Color = color;
        }
    }
}
