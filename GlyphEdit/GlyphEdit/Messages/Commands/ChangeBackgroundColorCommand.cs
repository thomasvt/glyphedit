using GlyphEdit.Model;

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
