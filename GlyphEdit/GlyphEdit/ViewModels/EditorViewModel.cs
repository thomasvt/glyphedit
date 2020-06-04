using System;
using System.Linq;
using GlyphEdit.Controls.DocumentView;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;
using GlyphEdit.Models;

namespace GlyphEdit.ViewModels
{
    /// <summary>
    /// I don't use standard MVVM because I don't like bidirectional databinding anymore.
    /// Instead, we use a fully separated "viewmodel" singleton and communicate towards controls using the MessageBus. The data is in dedicated, separate Models.
    /// </summary>
    public class EditorViewModel
    {
        private GlyphFont _glyphFont;
        private int _glyphIndex;
        private ColorPalette _colorPalette;

        private GlyphFontStore _glyphFontStore;
        private ColorPaletteStore _colorPaletteStore;

        public EditorViewModel()
        {
            BindCommandHandlers();
        }

        private void BindCommandHandlers()
        {
            MessageBus.Subscribe<NewDocumentCommand>(command => CreateNewDocument());
            MessageBus.Subscribe<ChangeGlyphFontCommand>(command => ChangeGlyph(command.GlyphFont));
            MessageBus.Subscribe<ChangeGlyphCommand>(command => ChangeGlyph(command.GlyphIndex));
            MessageBus.Subscribe<ChangeForegroundColorCommand>(c => ChangeForegroundColor(c.Color));
            MessageBus.Subscribe<ChangeBackgroundColorCommand>(c => ChangeBackgroundColor(c.Color));
            MessageBus.Subscribe<SetBrushGlyphEnabledCommand>(c => SetBrushGlyphEnabled(c.IsEnabled));
            MessageBus.Subscribe<SetBrushForegroundEnabledCommand>(c => SetBrushForegroundEnabled(c.IsEnabled));
            MessageBus.Subscribe<SetBrushBackgroundEnabledCommand>(c => SetBrushBackgroundEnabled(c.IsEnabled));
        }

        public void OnLoaded()
        {
            _glyphFontStore = new GlyphFontStore();
            _glyphFontStore.DetectGlyphFonts();
            _colorPaletteStore = new ColorPaletteStore();
            _colorPaletteStore.DetectColorPalettes();
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
            var firstFont = _glyphFontStore.GlyphFonts.OrderBy(gf => gf.FontName).ThenBy(gf => gf.GlyphSize.Y).First(gf => gf.IsValid);
            ChangeGlyph(firstFont, 1);
            ChangeColorPalette(_colorPaletteStore.ColorPalettes.OrderBy(cp => cp.Name).First(cp => cp.IsValid));
            ChangeEditMode(EditMode.Pencil);
            ChangeForegroundColor(new GlyphColor(255, 255, 255, 255));
            ChangeBackgroundColor(new GlyphColor(0, 0, 0, 255));
            SetBrushGlyphEnabled(true);
            SetBrushForegroundEnabled(true);
            SetBrushBackgroundEnabled(true);
        }

        public void ChangeGlyph(int glyphIndex)
        {
            if (_glyphFont == null)
                throw new Exception("No GlyphFont selected yet.");

            ChangeGlyph(_glyphFont, glyphIndex);
        }

        public void ChangeGlyph(GlyphFont glyphFont)
        {
            if (glyphFont == null)
                throw new ArgumentNullException(nameof(glyphFont));

            ChangeGlyph(glyphFont, _glyphIndex >= _glyphFont.GlyphCount ? 0 : _glyphIndex);
        }

        public void ChangeGlyph(GlyphFont glyphFont, int glyphIndex)
        {
            if (glyphFont == null)
                throw new ArgumentNullException(nameof(glyphFont));
            if (!_glyphFontStore.GlyphFonts.Contains(glyphFont))
                throw new Exception("Chosen GlyphFont is not known in the GlyphFontStore.");
            if (!glyphFont.IsValid)
                throw new Exception("Cannot use an invalid GlyphFont.");
            if (glyphIndex >= glyphFont.GlyphCount)
                throw new Exception($"GlyphFont has no glyph at index {glyphIndex}.");
            if (glyphFont.Equals(_glyphFont) && _glyphIndex == glyphIndex)
                return;

            var @event = new GlyphChangedEvent(_glyphFont, glyphFont, _glyphIndex, glyphIndex);
            _glyphFont = glyphFont;
            _glyphIndex = glyphIndex;
            MessageBus.Publish(@event);
        }

        private void ChangeColorPalette(ColorPalette colorPalette)
        {
            if (colorPalette == null)
                throw new ArgumentNullException(nameof(colorPalette));
            if (!_colorPaletteStore.ColorPalettes.Contains(colorPalette))
                throw new Exception("Chosen ColorPalette is not known in the ColorPaletteStore.");
            if (!colorPalette.IsValid)
                throw new Exception("Cannot use an invalid ColorPalette.");
            if (colorPalette.Equals(_colorPalette))
                return;

            var @event = new ColorPaletteChangedEvent(colorPalette);
            _colorPalette = colorPalette;
            MessageBus.Publish(@event);
        }

        private void ChangeForegroundColor(GlyphColor color)
        {
            MessageBus.Publish(new ForegroundColorChangedEvent(color));
        }

        private void ChangeBackgroundColor(GlyphColor color)
        {
            MessageBus.Publish(new BackgroundColorChangedEvent(color));
        }

        private void SetBrushGlyphEnabled(bool isEnabled)
        {
            MessageBus.Publish(new BrushGlyphEnabledChangedEvent(isEnabled));
        }

        private void SetBrushForegroundEnabled(bool isEnabled)
        {
            MessageBus.Publish(new BrushForegroundEnabledChangedEvent(isEnabled));
        }

        private void SetBrushBackgroundEnabled(bool isEnabled)
        {
            MessageBus.Publish(new BrushBackgroundEnabledChangedEvent(isEnabled));
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
