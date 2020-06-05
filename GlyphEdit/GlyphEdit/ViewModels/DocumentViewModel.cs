using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messaging;
using GlyphEdit.Model;
using GlyphEdit.Model.Persistence;
using Microsoft.Win32;

namespace GlyphEdit.ViewModels
{
    public class DocumentViewModel
    : IDisposable
    {
        public readonly Document Document;

        public DocumentViewModel(Document document)
        {
            Document = document;
            MessageBus.Subscribe<SaveDocumentCommand>(this, c => SaveDocument());
            MessageBus.Subscribe<SaveDocumentAsCommand>(this, c => SaveDocumentAs());
        }

        private void SaveDocument()
        {
            Debug.Write("c");
            if (Filename == null)
            {
                SaveDocumentAs();
                return;
            }
            DocumentSaver.Save(Document, Filename);
        }

        private void SaveDocumentAs()
        {
            Debug.Write("d");
            var dialog = new SaveFileDialog
            {
                DefaultExt = ".ged",
                Filter = "GlyphEdit Document (*.ged)|*.ged|All files (*.*)|*.*",
                FilterIndex = 0
            };
            if (Filename != null)
            {
                dialog.InitialDirectory = Path.GetDirectoryName(Filename) ?? Environment.CurrentDirectory;
                dialog.FileName = Path.GetFileName(Filename);
            }
            if (dialog.ShowDialog(Application.Current.MainWindow) == true)
            {
                Filename = dialog.FileName;
                DocumentSaver.Save(Document, Filename);
            }
        }

        public void Dispose()
        {
            MessageBus.Unsubscribe(this);
        }

        public string Filename { get; private set; }

        public bool IsModified { get; private set; }
    }
}
