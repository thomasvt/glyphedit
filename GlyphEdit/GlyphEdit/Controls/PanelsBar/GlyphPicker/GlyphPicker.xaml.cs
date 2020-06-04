﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GlyphEdit.Messages;
using GlyphEdit.Messaging;
using GlyphEdit.Model;
using GlyphEdit.ViewModel;

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
            foreach (var child in Grid.Children.OfType<ToggleButton>())
            {
                if (child.DataContext is int buttonGlyphIndex)
                {
                    child.IsChecked = buttonGlyphIndex == glyphIndex;
                }
                else
                {
                    throw new Exception("Bug: All togglebuttons should have GlyphIndex as DataContext.");
                }
            }
        }

        private void LoadGlyphFont(GlyphFont glyphFont)
        {
            if (_currentGlyphFontOfGlyphGrid == glyphFont)
                return;

            Grid.Children.Clear();

            Grid.Columns = (int)glyphFont.BitmapSource.Width / glyphFont.GlyphSize.X;
            Grid.Rows = (int)glyphFont.BitmapSource.Height / glyphFont.GlyphSize.Y;

            var glyphIndex = 0;
            for (var y = 0; y < Grid.Rows; y++)
            {
                for (var x = 0; x < Grid.Columns; x++)
                {
                    var imageSource = new CroppedBitmap(glyphFont.BitmapSource, new Int32Rect(x * glyphFont.GlyphSize.X, y * glyphFont.GlyphSize.Y, glyphFont.GlyphSize.X, glyphFont.GlyphSize.Y));
                    var button = new ToggleButton
                    {
                        Content = new Image
                        {
                            Stretch = Stretch.Uniform,
                            SnapsToDevicePixels = true,
                            Source = imageSource,
                        },
                        DataContext = glyphIndex
                    };
                    var buttonGlyphIndex = glyphIndex;
                    button.Click += (sender, args) => EditorViewModel.Current.ChangeGlyph(buttonGlyphIndex);
                    Grid.Children.Add(button);

                    glyphIndex++;
                }
            }

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
    }
}
