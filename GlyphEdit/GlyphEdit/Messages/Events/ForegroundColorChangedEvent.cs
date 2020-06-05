using GlyphEdit.Model;

namespace GlyphEdit.Messages.Events
{
    public class ForegroundColorChangedEvent
    {
        public GlyphColor Color { get; }

        public ForegroundColorChangedEvent(GlyphColor color)
        {
            Color = color;
        }
    }
}
