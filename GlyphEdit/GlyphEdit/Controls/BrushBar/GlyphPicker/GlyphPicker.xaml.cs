using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;
using GlyphEdit.ViewModels;

namespace GlyphEdit.Controls.BrushBar.GlyphPicker
{
    /// <summary>
    /// Interaction logic for GlyphPicker.xaml
    /// </summary>
    public partial class GlyphPicker : UserControl
    {
        private GlyphFontViewModel _currentGlyphFontViewModelOfGlyphGrid;

        public GlyphPicker()
        {
            InitializeComponent();

            MessageBus.Subscribe<GlyphFontListLoadedEvent>(e =>
            {
                GlyphFontPicker.ItemsSource = e.GlyphFonts.OrderBy(f => f.FontName).ThenBy(f => f.GlyphSize.Y);
            });
            MessageBus.Subscribe<GlyphChangedEvent>(e =>
            {
                GlyphFontPicker.SelectedItem = e.NewGlyphFontViewModel;
                ChangeGlyphGridFont(e.NewGlyphFontViewModel);
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

        private void ChangeGlyphGridFont(GlyphFontViewModel glyphFontViewModel)
        {
            if (_currentGlyphFontViewModelOfGlyphGrid == glyphFontViewModel)
                return;
            if (glyphFontViewModel.GlyphCount > 256)
                throw new Exception("Only fonts allowed of up to 256 characters.");

            var glyphButtonViewModels = new GlyphButtonViewModel[glyphFontViewModel.GlyphCount];
            for (var glyphIndex = 0; glyphIndex < glyphFontViewModel.GlyphCount; glyphIndex++)
            {
                if (glyphIndex >= glyphFontViewModel.GlyphCount)
                    break;

                var glyphCropInFontImage = glyphFontViewModel.GetGlyphCropRectangle(glyphIndex);
                glyphButtonViewModels[glyphIndex] = new GlyphButtonViewModel(glyphFontViewModel, glyphIndex, glyphCropInFontImage);
            }

            GlyphGrid.ItemsSource = glyphButtonViewModels;
            _currentGlyphFontViewModelOfGlyphGrid = glyphFontViewModel;
        }

        private void GlyphFontPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(GlyphFontPicker.SelectedItem is GlyphFontViewModel glyphFont))
                throw new Exception("ComboBoxItem datacontext should be a GlyphFont.");
            if (!glyphFont.IsValid)
            {
                // revert the change
                GlyphFontPicker.SelectedItem = _currentGlyphFontViewModelOfGlyphGrid;
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
