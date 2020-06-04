using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GlyphEdit.Messages;
using GlyphEdit.Messaging;
using GlyphEdit.Models;

namespace GlyphEdit.Controls.PanelsBar.GlyphPicker
{
    /// <summary>
    /// Interaction logic for GlyphPicker.xaml
    /// </summary>
    public partial class GlyphPicker : UserControl
    {
        private GlyphFont _currentGlyphFontOfGlyphGrid;

        public GlyphPicker()
        {
            InitializeComponent();

            MessageBus.Subscribe<GlyphFontListLoadedEvent>(e =>
            {
                GlyphFontPicker.ItemsSource = e.GlyphFonts.OrderBy(f => f.FontName).ThenBy(f => f.GlyphSize.Y);
            });
            MessageBus.Subscribe<GlyphChangedEvent>(e =>
            {
                GlyphFontPicker.SelectedItem = e.NewGlyphFont;
                LoadGlyphFont(e.NewGlyphFont);
                SelectGlyphButton(e.NewGlyphIndex);
            });
        }

        private void SelectGlyphButton(int glyphIndex)
        {
            foreach (var child in GlyphGrid.ItemsSource.OfType<GlyphButtonViewModel>())
            {
                child.IsPicked = child.GlyphIndex == glyphIndex;
            }
        }

        private void LoadGlyphFont(GlyphFont glyphFont)
        {
            if (_currentGlyphFontOfGlyphGrid == glyphFont)
                return;
            if (glyphFont.GlyphCount > 256)
                throw new Exception("Only fonts allowed of up to 256 characters.");

            var fontColumnCount = glyphFont.BitmapSource.PixelWidth / glyphFont.GlyphSize.X;

            var glyphButtonViewModels = new GlyphButtonViewModel[glyphFont.GlyphCount];
            for (var glyphIndex = 0; glyphIndex < glyphFont.GlyphCount; glyphIndex++)
            {
                if (glyphIndex >= glyphFont.GlyphCount)
                    continue;

                var x = glyphIndex % fontColumnCount;
                var y = glyphIndex / fontColumnCount;
                var glyphCropInFontImage = new Int32Rect(x * glyphFont.GlyphSize.X, y * glyphFont.GlyphSize.Y, glyphFont.GlyphSize.X, glyphFont.GlyphSize.Y);
                glyphButtonViewModels[glyphIndex] = new GlyphButtonViewModel(glyphFont, glyphIndex, glyphCropInFontImage);
            }

            GlyphGrid.ItemsSource = glyphButtonViewModels;
            _currentGlyphFontOfGlyphGrid = glyphFont;
        }

        private void GlyphFontPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(GlyphFontPicker.SelectedItem is GlyphFont glyphFont))
                throw new Exception("ComboBoxItem datacontext should be a GlyphFont.");
            if (!glyphFont.IsValid)
            {
                // revert the change
                GlyphFontPicker.SelectedItem = _currentGlyphFontOfGlyphGrid;
                MessageBox.Show("Cannot use an invalid font: " + glyphFont.Error, "Invalid font", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            }
            else
            {
                MessageBus.Publish(new ChangeGlyphFontCommand(glyphFont));
            }
        }

        private void GlyphButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is ToggleButton glyphButton))
                throw new Exception("Bug: should be called by Glyphbuttons only.");
            MessageBus.Publish(new ChangeGlyphCommand((glyphButton.DataContext as GlyphButtonViewModel).GlyphIndex));
        }
    }
}
