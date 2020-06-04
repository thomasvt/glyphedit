using GlyphEdit.Models;

namespace GlyphEdit.Messages.Commands
{
    public class ChangeBackgroundColorCommand
    {
        public readonly GlyphColor Color;

        public ChangeBackgroundColorCommand(GlyphColor color)
        {
            Color = color;
        }
    }
}
