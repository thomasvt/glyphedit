namespace GlyphEdit.Messages.Commands
{
    public class SetBrushGlyphEnabledCommand
    {
        public readonly bool IsEnabled;

        public SetBrushGlyphEnabledCommand(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}
