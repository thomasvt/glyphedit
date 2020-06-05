using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using GlyphEdit.Messages;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;
using GlyphEdit.Model;

namespace GlyphEdit.Controls.PanelsBar.PalettePicker
{
    /// <summary>
    /// Interaction logic for PalettePicker.xaml
    /// </summary>
    public partial class PalettePicker : UserControl
    {
        public PalettePicker()
        {
            InitializeComponent();

            MessageBus.Subscribe<ColorPaletteChangedEvent>(e => { ShowColorPalette(e.ColorPalette); });
        }

        private void ShowColorPalette(ColorPalette colorPalette)
        {
            var colorViewModels = new List<ColorButtonViewModel>(colorPalette.ColumnCount * colorPalette.RowCount);
            for (var y = 0; y < colorPalette.RowCount; y++) // row first because the UniformGrid works like that
            { 
                for (var x = 0; x < colorPalette.ColumnCount; x++)
                {
                    colorViewModels.Add(new ColorButtonViewModel(colorPalette.Colors[x, y]));
                }
            }
            ColorGrid.ItemsSource = colorViewModels;
        }

        private void ColorSwatch_Click(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Control colorButton))
                throw new Exception("Bug: should be called by ColorPaletteButtons only.");

            if (e.ChangedButton == MouseButton.Left)
                MessageBus.Publish(new ChangeForegroundColorCommand((colorButton.DataContext as ColorButtonViewModel).GlyphGlyphColor));
            else if (e.ChangedButton == MouseButton.Right)
                MessageBus.Publish(new ChangeBackgroundColorCommand((colorButton.DataContext as ColorButtonViewModel).GlyphGlyphColor));
        }
    }
}
