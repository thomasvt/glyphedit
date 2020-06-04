using GlyphEdit.Models;

namespace GlyphEdit.Messages.Events
{
    public class BackgroundColorChangedEvent
    {
        public GlyphColor Color { get; }

        public BackgroundColorChangedEvent(GlyphColor color)
        {
            Color = color;
        }
    }
}
