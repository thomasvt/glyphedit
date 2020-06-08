using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;
using GlyphEdit.Model;
using GlyphEdit.Wpf.ColorGrid;

namespace GlyphEdit.Controls.BrushBar.PalettePicker
{
    /// <summary>
    /// Interaction logic for ColorPanel.xaml
    /// </summary>
    public partial class ColorPanel : UserControl
    {
        private ColorPalette _currentColorPalette;

        public ColorPanel()
        {
            InitializeComponent();

            MessageBus.Subscribe<ColorPaletteChangedEvent>(e => { ShowColorPalette(e.ColorPalette); });
        }

        private void ShowColorPalette(ColorPalette colorPalette)
        {
            _currentColorPalette = colorPalette;
            var colorPatches = new List<ColorPatch>();
            for (var y = 0; y < colorPalette.RowCount; y++) // row first because the UniformGrid works like that
            {
                for (var x = 0; x < colorPalette.ColumnCount; x++)
                {
                    var color = colorPalette.Colors[x, y];
                    if (color.A > 0)
                        colorPatches.Add(new ColorPatch(color.ToWpfColor(), x, y) { Tag = color });
                }
            }

            ColorGrid.ColumnCount = colorPalette.ColumnCount;
            ColorGrid.RowCount = colorPalette.RowCount;
            ColorGrid.ColorPatches = colorPatches;
        }

        private void ColorGrid_OnColorPatchLeftClick(object sender, ColorPatchRoutedEventArgs e)
        {
            MessageBus.Publish(new ChangeForegroundColorCommand((GlyphColor)e.ColorPatch.Tag));
        }

        private void ColorGrid_OnColorsModified(object sender, RoutedEventArgs e)
        {
            UpdateColorPaletteFromGrid();
            MessageBus.Publish(new SaveCurrentColorPaletteCommand());
        }

        private void UpdateColorPaletteFromGrid()
        {
            if (_currentColorPalette == null)
                throw new Exception("No ColorPalette selected.");

            var newColors = new GlyphColor[ColorGrid.ColumnCount, ColorGrid.RowCount];

            // fill with patches in Grid
            foreach (var colorPatch in ColorGrid.ColorPatches)
            {
                newColors[colorPatch.Column, colorPatch.Row] = colorPatch.Color.ToGlyphColor();
            }

            _currentColorPalette.ChangeColors(newColors);
        }
    }
}
