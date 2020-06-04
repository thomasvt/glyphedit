namespace GlyphEdit.Messages.Events
{
    public class BrushBackgroundEnabledChangedEvent
    {
        public readonly bool IsEnabled;

        public BrushBackgroundEnabledChangedEvent(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}
