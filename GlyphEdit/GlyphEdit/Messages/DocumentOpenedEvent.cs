using System.Diagnostics;
using GlyphEdit.Models;

namespace GlyphEdit.Messages
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
