using System;
using System.IO;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messaging;
using GlyphEdit.Models;
using GlyphEdit.Persistence;
using Microsoft.Win32;

namespace GlyphEdit.ViewModels
{
    public class DocumentViewModel
    {
        public readonly Document Document;

        public DocumentViewModel(Document document)
        {
            Document = document;
            MessageBus.Subscribe<SaveDocumentCommand>(c => SaveDocument());
            MessageBus.Subscribe<SaveDocumentAsCommand>(c => SaveDocumentAs());
        }

        private void SaveDocument()
        {
            if (Filename == null)
            {
                SaveDocumentAs();
                return;
            }
            DocumentSaver.Save(Document, Filename);
        }

        private void SaveDocumentAs()
        {
            var dialog = new SaveFileDialog
            {
                InitialDirectory = Filename != null ? Path.GetDirectoryName(Filename) : Environment.CurrentDirectory,
                FileName = Filename != null ? Path.GetFileName(Filename) : null,
                DefaultExt = ".ged",
                Filter = "GlyphEdit Document (*.ged)|*.ged|All files (*.*)|*.*",
                FilterIndex = 0
            };
            if (dialog.ShowDialog() == true)
            {
                Filename = dialog.FileName;
                DocumentSaver.Save(Document, Filename);
            }
        }

        public string Filename { get; private set; }
        public bool IsModified { get; private set; }
    }
}
