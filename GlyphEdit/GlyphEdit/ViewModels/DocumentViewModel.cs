using System;
using System.IO;
using System.Windows;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messages.Events;
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

        public DocumentViewModel(Document document, string filename)
        {
            Filename = filename;
            Document = document;
            Document.Manipulator.UndoStackChanged += (sender, args) => MessageBus.Publish(new UndoStackStateChangedEvent(Document.Manipulator.CanUndo(), Document.Manipulator.CanRedo()));
            MessageBus.Publish(new UndoStackStateChangedEvent(Document.Manipulator.CanUndo(), Document.Manipulator.CanRedo()));

            MessageBus.Subscribe<SaveDocumentCommand>(this, c => DoSaveWorkflow());
            MessageBus.Subscribe<SaveDocumentAsCommand>(this, c => DoSaveAsWorkflow());
            MessageBus.Subscribe<UndoCommand>(this, c => Undo());
            MessageBus.Subscribe<RedoCommand>(this, c => Redo());
            MessageBus.Subscribe<ZoomToCommand>(c => ZoomTo(c.Percentage));
        }

        private void DoSaveWorkflow()
        {
            if (Filename == null)
            {
                DoSaveAsWorkflow();
                return;
            }

            SaveDocument();
        }

        private void DoSaveAsWorkflow()
        {
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
                MessageBus.Publish(new DocumentFilenameChangedEvent(Filename));
                SaveDocument();
            }
        }

        private void SaveDocument()
        {
            DocumentSaver.Save(Document, Filename);
            Document.Manipulator.ResetUndoStack();

            MessageBus.Publish(new DocumentSavedEvent());
        }

        private void ZoomTo(float zoom)
        {
            MessageBus.Publish(new ZoomChangeRequestedEvent(zoom));
        }

        private void Undo()
        {
            Document.Manipulator.Undo();
        }

        private void Redo()
        {
            Document.Manipulator.Redo();
        }

        public void Dispose()
        {
            MessageBus.Unsubscribe(this);
        }

        public string Filename { get; private set; }
        public Guid ActiveLayer { get; private set; }
        public bool DocumentIsModified => Document.Manipulator.CanUndo();
    }
}
