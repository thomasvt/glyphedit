namespace GlyphEdit.Messages.Events
{
    public class BrushForegroundEnabledChangedEvent
    {
        public readonly bool IsEnabled;

        public BrushForegroundEnabledChangedEvent(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}
