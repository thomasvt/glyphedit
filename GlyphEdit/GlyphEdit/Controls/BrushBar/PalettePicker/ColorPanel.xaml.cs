using System;
using System.Collections.Generic;
using System.Timers;
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
using Colors = GlyphEdit.Model.Colors;

namespace GlyphEdit.Controls.BrushBar.PalettePicker
{
    /// <summary>
    /// Interaction logic for ColorPanel.xaml
    /// </summary>
    public partial class ColorPanel : UserControl
    {
        private ColorPalette _currentColorPalette;
        private readonly Timer _contextMenuTriggerTimer;
        private Point _contextMenuMouseLocation;
        private ColorPatch _contextMenuColorPatch;

        public ColorPanel()
        {
            InitializeComponent();

            _contextMenuTriggerTimer = new Timer(400);
            _contextMenuTriggerTimer.Elapsed += (o, args) =>
            {
                _contextMenuTriggerTimer.Stop();

                Dispatcher.Invoke(OpenColorPatchContextMenu);
            };
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
            MessageBus.Publish(new ChangeForegroundColorCommand((GlyphColor)e.ColorPatch.Color.ToGlyphColor()));
        }

        private void ColorGrid_OnColorPatchRightClick(object sender, ColorPatchRoutedEventArgs e)
        {
            MessageBus.Publish(new ChangeBackgroundColorCommand((GlyphColor)e.ColorPatch.Color.ToGlyphColor()));
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

            _contextMenuTriggerTimer.Start();

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

        private void ColorGrid_OnMouseMove(object sender, MouseEventArgs e)
        {
            _contextMenuTriggerTimer.Stop();
        }

        private void ColorGrid_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _contextMenuTriggerTimer.Stop();
        }

        private void CopyHexCode_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (sender as FrameworkElement).DataContext as ColorGridContextMenuViewModel;
            Clipboard.SetText(viewModel.ColorHexCode);
        }

        private void PasteHexCode_Click(object sender, RoutedEventArgs e)
        {
            var text = Clipboard.GetText();
            var color = Colors.FromHex(Clipboard.GetText()).ToWpfColor();
            if (Colors.IsHexCode(text))
            {
                var control = sender as FrameworkElement;
                if (control.DataContext is ColorGridContextMenuViewModel viewModel)
                {
                    _contextMenuColorPatch.Color = color;
                    SaveAndRefreshPalette();
                }
                else if (control.DataContext is ColorGridNoPatchContextMenuViewModel noPatchViewModel)
                {
                    CreatePatch(noPatchViewModel.MouseLocation, color);
                }
            }
            else
            {
                MessageBox.Show("No color hexcode on the clipboard.", "Invalid hexcolor", MessageBoxButton.OK, MessageBoxImage.Error);
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
            var colorPicker = new ColorPicker();
            colorPicker.ShowDialog();
        }
    }
}
