using System;
using System.IO;
using System.Linq;
using GlyphEdit.Controls.DocumentView;
using GlyphEdit.Messages;
using GlyphEdit.Messaging;
using GlyphEdit.Model;

namespace GlyphEdit.ViewModel
{
    /// <summary>
    /// I don't use standard MVVM because I don't like bidirectional databinding anymore.
    /// Instead, we use a fully separated "viewmodel" singleton and communicate towards controls using the MessageBus. The data is in dedicated, separate Models.
    /// </summary>
    public class EditorViewModel
    {
        private GlyphFont _glyphFont;
        private GlyphFontStore _glyphFontStore;

        public EditorViewModel()
        {
            BindCommandHandlers();
        }

        private void BindCommandHandlers()
        {
            MessageBus.Subscribe<NewDocumentCommand>(command => CreateNewDocument());
        }

        public void OnLoaded()
        {
            _glyphFontStore = new GlyphFontStore();
            _glyphFontStore.Initialize();
            CreateNewDocument();
        }

        public void CreateNewDocument()
        {
            Document = new Document(50, 50);
            ResetUI();
            MessageBus.Publish(new DocumentOpenedEvent(Document));
        }

        private void ResetUI()
        {
            if (!_glyphFontStore.GlyphFonts.Any())
                throw new Exception("No GlyphFonts found. GlyphEdit cannot function without.");
            ChangeGlyph(_glyphFontStore.GlyphFonts.First(), 0);
            ChangeEditMode(EditMode.Pencil);
        }

        public void ChangeGlyph(int glyphIndex)
        {
            if (_glyphFont == null)
                throw new Exception("No GlyphFont selected yet.");

            ChangeGlyph(_glyphFont, glyphIndex);
        }

        public void ChangeGlyph(GlyphFont glyphFont, int glyphIndex)
        {
            if (glyphFont.GlyphCount <= glyphIndex)
                throw new Exception($"GlyphFont has no glyph at index {glyphIndex}.");

            _glyphFont = glyphFont;
            MessageBus.Publish(new GlyphChangedEvent(_glyphFont, glyphIndex));
        }

        public void ChangeEditMode(EditMode editMode)
        {
            if (editMode != EditMode)
            {
                EditMode = editMode;
                MessageBus.Publish(new EditModeChangedEvent(editMode));
            }
        }

        public Document Document { get; private set; }

        public EditMode EditMode { get; private set; }

        public static EditorViewModel Current = new EditorViewModel();
        
    }
}
