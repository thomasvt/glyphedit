namespace GlyphEdit.Messages.Commands
{
    public class ZoomToCommand
    {
        public readonly float Percentage;

        public ZoomToCommand(float percentage)
        {
            Percentage = percentage;
        }
    }
}
