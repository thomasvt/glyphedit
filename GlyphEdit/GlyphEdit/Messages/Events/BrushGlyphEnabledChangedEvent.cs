namespace GlyphEdit.Messages.Events
{
    public class BrushGlyphEnabledChangedEvent
    {
        public readonly bool IsEnabled;

        public BrushGlyphEnabledChangedEvent(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}
