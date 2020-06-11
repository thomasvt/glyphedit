using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;
using GlyphEdit.Model;
using GlyphEdit.Wpf;
using GlyphEdit.Wpf.ColorGrid;

namespace GlyphEdit.Controls.BrushBar.PalettePicker
{
    /// <summary>
    /// Interaction logic for ColorPanel.xaml
    /// </summary>
    public partial class ColorPanel : UserControl
    {
        private ColorPalette _currentColorPalette;
        private Point _contextMenuMouseLocation;
        private ColorPatch _contextMenuColorPatch;

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
                        colorPatches.Add(new ColorPatch(color.ToWpfColor(), new PointI(x, y)));
                }
            }

            ColorGrid.ColumnCount = colorPalette.ColumnCount;
            ColorGrid.RowCount = colorPalette.RowCount;
            ColorGrid.ColorPatches = colorPatches;
        }

        private void ColorGrid_OnColorPatchLeftClick(object sender, ColorPatchRoutedEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                MessageBus.Publish(new ChangeBackgroundColorCommand(e.ColorPatch.Color.ToGlyphColor()));
            else
                MessageBus.Publish(new ChangeForegroundColorCommand(e.ColorPatch.Color.ToGlyphColor()));
        }

        private void ColorGrid_OnColorPatchRightClick(object sender, ColorPatchRoutedEventArgs e)
        {
            _contextMenuColorPatch = e.ColorPatch;
            OpenColorPatchContextMenu();
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
                newColors[colorPatch.GridLocation.X, colorPatch.GridLocation.Y] = colorPatch.Color.ToGlyphColor();
            }

            _currentColorPalette.ChangeColors(newColors);
        }

        private void ColorGrid_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _contextMenuMouseLocation = e.GetPosition(ColorGrid);
            _contextMenuColorPatch = ColorGrid.GetColorPatchAt(e.GetPosition(ColorGrid));
            OpenColorPatchContextMenu();
        }

        private void OpenColorPatchContextMenu()
        {
            ContextMenu contextMenu;
            if (_contextMenuColorPatch == null)
            {
                contextMenu = this.FindResource("ColorGridNoPatchContextMenu") as ContextMenu;
                contextMenu.DataContext = new ColorGridNoPatchContextMenuViewModel
                {
                    MouseLocation = _contextMenuMouseLocation
                };
            }
            else
            {
                contextMenu = this.FindResource("ColorGridContextMenu") as ContextMenu;
                contextMenu.DataContext = new ColorGridContextMenuViewModel
                {
                    ColorPatchBrush = new SolidColorBrush(_contextMenuColorPatch.Color),
                    ColorHexCode = _contextMenuColorPatch.Color.ToHexString()
                };
            }
            contextMenu.PlacementTarget = ColorGrid;
            contextMenu.IsOpen = true;
        }
        
        private void CopyHexCode_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (sender as FrameworkElement).DataContext as ColorGridContextMenuViewModel;
            Clipboard.SetText(viewModel.ColorHexCode);
        }

        private void PasteHexCode_Click(object sender, RoutedEventArgs e)
        {
            var text = Clipboard.GetText();
            if (ColorUtils.IsHexCode(text))
            {
                var color = ColorUtils.FromHex(Clipboard.GetText());
                CreateOrChangeColor(color, _contextMenuColorPatch, _contextMenuMouseLocation);
            }
            else
            {
                MessageBox.Show("No color hexcode on the clipboard.", "Invalid hexcolor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateOrChangeColor(Color color, ColorPatch colorPatch, Point location)
        {
            if (colorPatch != null)
            {
                colorPatch.Color = color;
                SaveAndRefreshPalette();
            }
            else
            {
                CreatePatch(location, color);
            }
        }

        public void CreatePatch(Point location, Color color)
        {
            if (ColorGrid.GetColorPatchAt(location) != null)
                throw new Exception("There already is a colorpatch there.");

            var list = ColorGrid.ColorPatches;
            var gridLocation = ColorGrid.GetGridLocationAt(location);
            list.Add(new ColorPatch(color, gridLocation));

            SaveAndRefreshPalette();
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (_contextMenuColorPatch == null)
                throw new Exception("No colorpatch selected.");

            var list = ColorGrid.ColorPatches;
            list.Remove(_contextMenuColorPatch);

            SaveAndRefreshPalette();
        }

        private void SaveAndRefreshPalette()
        {
            UpdateColorPaletteFromGrid();
            ColorGrid.InvalidateVisual(); // this is a bit messy to update the colors in the ColorGrid by re-set-ting the property with a changed collection. Should be ObservableCollection but meh...
            MessageBus.Publish(new SaveCurrentColorPaletteCommand());
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var color = _contextMenuColorPatch?.Color ?? Colors.Black;
            var colorPicker = new ColorPicker
            {
                Color = new HslRgbColor(color.R, color.G, color.B)
            };
            if (colorPicker.ShowDialog() == true)
            {
                CreateOrChangeColor(colorPicker.Color.ToWpfColor(), _contextMenuColorPatch, _contextMenuMouseLocation);
            }
        }

        private void ShrinkPaletteButton_Click(object sender, RoutedEventArgs e)
        {
            var bottomRow = ColorGrid.RowCount - 1;
            if (ColorGrid.RowCount < 2 || ColorGrid.ColorPatches.Any(p => p.GridLocation.Y == bottomRow))
                return;
            ColorGrid.RowCount--;
            SaveAndRefreshPalette();
        }

        private void GrowPaletteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ColorGrid.RowCount == 20)
                return;
            ColorGrid.RowCount++;
            SaveAndRefreshPalette();
        }
    }
}
