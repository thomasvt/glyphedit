namespace GlyphEdit.Messages.Commands
{
    public class SetBrushForegroundEnabledCommand
    {
        public readonly bool IsEnabled;

        public SetBrushForegroundEnabledCommand(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}
