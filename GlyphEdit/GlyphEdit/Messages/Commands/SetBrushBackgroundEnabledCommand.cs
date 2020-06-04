namespace GlyphEdit.Messages.Commands
{
    public class SetBrushBackgroundEnabledCommand
    {
        public readonly bool IsEnabled;

        public SetBrushBackgroundEnabledCommand(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}
