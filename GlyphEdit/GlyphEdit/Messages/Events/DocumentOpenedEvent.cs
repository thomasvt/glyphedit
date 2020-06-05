using System.Diagnostics;
using GlyphEdit.Model;

namespace GlyphEdit.Messages.Events
{
    public class DocumentOpenedEvent
    {
        public readonly Document Document;

        [DebuggerStepThrough]
        public DocumentOpenedEvent(Document document)
        {
            Document = document;
        }
    }
}
