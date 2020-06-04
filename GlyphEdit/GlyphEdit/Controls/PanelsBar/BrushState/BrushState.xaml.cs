using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;
using GlyphEdit.ViewModels;

namespace GlyphEdit.Controls.PanelsBar.BrushState
{
    /// <summary>
    /// Interaction logic for GlyphVisualisation.xaml
    /// </summary>
    public partial class BrushState : UserControl
    {
        private Color _foregroundColor;
        private BitmapSource _glyphBitmap;

        public BrushState()
        {
            InitializeComponent();
            _foregroundColor = System.Windows.Media.Colors.White;

            MessageBus.Subscribe<GlyphChangedEvent>(e =>
                {
                    _glyphBitmap = new CroppedBitmap(e.NewGlyphFont.BitmapSource, e.NewGlyphFont.GetGlyphCropRectangle(e.NewGlyphIndex));
                    var bitmap = BitmapUtils.ReplaceColor(_glyphBitmap, System.Windows.Media.Colors.White, _foregroundColor);
                    BrushStateGlyphImage.Source = bitmap;
                });
            MessageBus.Subscribe<ForegroundColorChangedEvent>(e =>
            {
                _foregroundColor = e.Color.ToWpfColor();
                BrushStateGlyphImage.Source = BitmapUtils.ReplaceColor(_glyphBitmap, System.Windows.Media.Colors.White, _foregroundColor);
            });
            MessageBus.Subscribe<BackgroundColorChangedEvent>(e =>
            {
                var backgroundColor = e.Color.ToWpfColor();
                BrushStateBackground.Fill = new SolidColorBrush(backgroundColor);
            });
            MessageBus.Subscribe<BrushGlyphEnabledChangedEvent>(e => GlyphToggleButton.IsChecked  = e.IsEnabled);
            MessageBus.Subscribe<BrushForegroundEnabledChangedEvent>(e => ForegroundToggleButton.IsChecked = e.IsEnabled);
            MessageBus.Subscribe<BrushBackgroundEnabledChangedEvent>(e => BackgroundToggleButton.IsChecked = e.IsEnabled);
        }

        private void GlyphToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is ToggleButton toggleButton))
                throw new Exception("Should only be called by a togglebutton.");
            MessageBus.Publish(new SetBrushGlyphEnabledCommand(toggleButton.IsChecked ?? false));
        }

        private void FrontToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is ToggleButton toggleButton))
                throw new Exception("Should only be called by a togglebutton.");
            MessageBus.Publish(new SetBrushForegroundEnabledCommand(toggleButton.IsChecked ?? false));
        }

        private void BackToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is ToggleButton toggleButton))
                throw new Exception("Should only be called by a togglebutton.");
            MessageBus.Publish(new SetBrushBackgroundEnabledCommand(toggleButton.IsChecked ?? false));
        }
    }
}
