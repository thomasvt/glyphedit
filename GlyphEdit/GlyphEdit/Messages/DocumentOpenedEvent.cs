using GlyphEdit.Model;

namespace GlyphEdit.Messages
{
    public class DocumentOpenedEvent
    {
        public readonly Document Document;

        public DocumentOpenedEvent(Document document)
        {
            Document = document;
        }
    }
}
