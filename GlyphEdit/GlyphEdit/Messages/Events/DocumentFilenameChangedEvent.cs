namespace GlyphEdit.Messages.Events
{
    public class DocumentFilenameChangedEvent
    {
        public string Filename { get; }

        public DocumentFilenameChangedEvent(string filename)
        {
            Filename = filename;
        }
    }
}
