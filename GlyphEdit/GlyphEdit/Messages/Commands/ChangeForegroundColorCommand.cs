using GlyphEdit.Model;

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
