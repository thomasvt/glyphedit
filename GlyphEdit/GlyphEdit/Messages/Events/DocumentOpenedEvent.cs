using System.Diagnostics;
using GlyphEdit.ViewModels;

namespace GlyphEdit.Messages.Events
{
    public class DocumentOpenedEvent
    {
        public readonly DocumentViewModel Document;

        [DebuggerStepThrough]
        public DocumentOpenedEvent(DocumentViewModel document)
        {
            Document = document;
        }
    }
}
