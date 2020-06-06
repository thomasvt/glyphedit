using System;
using System.Linq;
using System.Windows;
using GlyphEdit.Controls.DocumentControl.EditTools;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;
using GlyphEdit.Model;
using GlyphEdit.Model.Persistence;
using GlyphEdit.ViewModels.Workflows;
using Microsoft.Win32;

namespace GlyphEdit.ViewModels
{
    public class EditorViewModel
    {
        private GlyphFontViewModel _glyphFontViewModel;
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
            MessageBus.Subscribe<NewDocumentCommand>(command => DoNewDocumentWorkflow());
            MessageBus.Subscribe<OpenDocumentCommand>(command => OpenDocumentFromFile());
            MessageBus.Subscribe<ChangeGlyphFontCommand>(command => ChangeGlyph(command.GlyphFontViewModel));
            MessageBus.Subscribe<ChangeGlyphCommand>(command => ChangeGlyph(command.GlyphIndex));
            MessageBus.Subscribe<ChangeForegroundColorCommand>(c => ChangeForegroundColor(c.Color));
            MessageBus.Subscribe<ChangeBackgroundColorCommand>(c => ChangeBackgroundColor(c.Color));
            MessageBus.Subscribe<SetBrushGlyphEnabledCommand>(c => SetBrushGlyphEnabled(c.IsEnabled));
            MessageBus.Subscribe<SetBrushForegroundEnabledCommand>(c => SetBrushForegroundEnabled(c.IsEnabled));
            MessageBus.Subscribe<SetBrushBackgroundEnabledCommand>(c => SetBrushBackgroundEnabled(c.IsEnabled));
            MessageBus.Subscribe<ExitApplicationCommand>(c => DoExitApplicationWorkflow());
        }

        /// <summary>
        /// Triggered by UI when UI is initialized, WPF measured+arranged, and the DirectX render control up and running.
        /// </summary>
        public void OnLoaded()
        {
            _glyphFontStore = new GlyphFontStore();
            _glyphFontStore.DetectGlyphFonts();
            _colorPaletteStore = new ColorPaletteStore();
            _colorPaletteStore.DetectColorPalettes();
            CreateAndOpenNewDocument(50, 50);
        }

        private void DoExitApplicationWorkflow()
        {
            if (!DoCloseDocumentWorkflow()) 
                return; // closing was canceled
            MessageBus.Publish(new ExitApplicationEvent());
        }

        /// <summary>
        /// Checks if the current document can be closed. Asks for save if modified. Returns false if the user chooses to cancel the close.
        /// </summary>
        private bool DoCloseDocumentWorkflow()
        {
            if (DocumentViewModel.DocumentIsModified)
            {
                var result = MessageBox.Show("Save your changes before closing?", "Close application",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Cancel)
                    return false;
                if (result == MessageBoxResult.Yes)
                {
                    if (!DocumentViewModel.DoSaveWorkflow())
                        return false;
                }
            }

            return true;
        }

        private void OpenDocumentFromFile()
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".ged",
                Filter = "GlyphEdit Document (*.ged)|*.ged|All files (*.*)|*.*",
                FilterIndex = 0
            };
            if (dialog.ShowDialog(App.Current.MainWindow) == true)
            {
                var document = DocumentLoader.Load(dialog.FileName);
                OpenDocument(document, dialog.FileName);
            }
        }

        public void DoNewDocumentWorkflow()
        {
            var newDocumentDialog = new NewDocumentDialog();
            if (newDocumentDialog.ShowDialog() == true)
            {
                CreateAndOpenNewDocument(50, 50);
            }
        }

        private void CreateAndOpenNewDocument(int width, int height)
        {
            var document = new Document(width, height);
            OpenDocument(document);
        }

        private void OpenDocument(Document document, string filename = null)
        {
            DocumentViewModel?.Dispose();
            DocumentViewModel = new DocumentViewModel(document, filename);

            ResetUI();
            MessageBus.Publish(new DocumentOpenedEvent(DocumentViewModel));
        }

        private void ResetUI()
        {
            if (!_glyphFontStore.GlyphFonts.Any())
                throw new Exception("No GlyphFonts found. GlyphEdit cannot function without.");
            var firstFont = _glyphFontStore.GlyphFonts.OrderBy(gf => gf.FontName).ThenBy(gf => gf.GlyphSize.Y).First(gf => gf.IsValid);
            ChangeGlyph(firstFont, 1);
            ChangeColorPalette(_colorPaletteStore.ColorPalettes.OrderBy(cp => cp.Name).First(cp => cp.IsValid));
            ChangeEditMode(EditMode.Pencil);
            ChangeBackgroundColor(new GlyphColor(255, 255, 255, 255));
            ChangeForegroundColor(new GlyphColor(0, 0, 0, 255));
            SetBrushGlyphEnabled(true);
            SetBrushForegroundEnabled(true);
            SetBrushBackgroundEnabled(true);
        }

        public void ChangeGlyph(int glyphIndex)
        {
            if (_glyphFontViewModel == null)
                throw new Exception("No GlyphFont selected yet.");

            ChangeGlyph(_glyphFontViewModel, glyphIndex);
        }

        public void ChangeGlyph(GlyphFontViewModel glyphFontViewModel)
        {
            if (glyphFontViewModel == null)
                throw new ArgumentNullException(nameof(glyphFontViewModel));

            ChangeGlyph(glyphFontViewModel, _glyphIndex >= _glyphFontViewModel.GlyphCount ? 0 : _glyphIndex);
        }

        public void ChangeGlyph(GlyphFontViewModel glyphFontViewModel, int glyphIndex)
        {
            if (glyphFontViewModel == null)
                throw new ArgumentNullException(nameof(glyphFontViewModel));
            if (!_glyphFontStore.GlyphFonts.Contains(glyphFontViewModel))
                throw new Exception("Chosen GlyphFont is not known in the GlyphFontStore.");
            if (!glyphFontViewModel.IsValid)
                throw new Exception("Cannot use an invalid GlyphFont.");
            if (glyphIndex >= glyphFontViewModel.GlyphCount)
                throw new Exception($"GlyphFont has no glyph at index {glyphIndex}.");
            if (glyphFontViewModel.Equals(_glyphFontViewModel) && _glyphIndex == glyphIndex)
                return;

            var @event = new GlyphChangedEvent(_glyphFontViewModel, glyphFontViewModel, _glyphIndex, glyphIndex);
            _glyphFontViewModel = glyphFontViewModel;
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

        public DocumentViewModel DocumentViewModel { get; private set; }

        public EditMode EditMode { get; private set; }

        public static EditorViewModel Current = new EditorViewModel();
    }
}
