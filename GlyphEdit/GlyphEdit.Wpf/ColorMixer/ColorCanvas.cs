using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GlyphEdit.Wpf.ColorMixer
{
    /// <summary>
    /// The rainbow-ish color picker canvas with Hue on X axis and Light on Y axis. Don't place it in infinite size parents (eg. scrollable area)
    /// </summary>
    public class ColorCanvas : Control
    {
        protected override Size MeasureOverride(Size constraint)
        {
            return constraint; // we take what we get... 
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            // calculate pixels of color canvas
            var pixels = new byte[(int)ActualWidth * (int)ActualHeight * 4];
            
            var saturation = Saturation;

            var width = (int)ActualWidth;
            var height = (int)ActualHeight;
            var i = 0;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var luminance = 1f - (double)y / height;
                    var hue = x * 365d / width;
                    var color = new HslRgbColor(hue, saturation, luminance);

                    pixels[i++] = color.B;
                    pixels[i++] = color.G;
                    pixels[i++] = color.R;
                    i++; // Bgr32 format ignores 4th byte
                }
            }

            if (_bitmap == null || (_bitmap.PixelWidth == width && _bitmap.PixelHeight == height))
                _bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null); // heavy ctor, reuse instance
            _bitmap.WritePixels(new Int32Rect(0, 0, (int)ActualWidth, (int)ActualHeight), pixels, (int)ActualWidth * 4, 0);
            drawingContext.DrawImage(_bitmap, new Rect(RenderSize));
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            PickColor(e.GetPosition(this));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                PickColor(e.GetPosition(this));
        }

        private void PickColor(Point position)
        {
            var hue = position.X * 365d / ActualWidth;
            var luminance = 1f - position.Y / ActualHeight;

            RaiseEvent(new ColorRoutedEventArgs(ColorPickedEvent, new HslRgbColor(hue, Saturation, luminance)));
        }

        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
            "Saturation", typeof(double), typeof(ColorCanvas), new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsRender));

        private WriteableBitmap _bitmap;

        public double Saturation
        {
            get => (double)GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }

        #region Routed Events

        public delegate void ColorRoutedEventHandler(object sender, ColorRoutedEventArgs e);

        public static readonly RoutedEvent ColorPickedEvent = EventManager.RegisterRoutedEvent(
            "ColorPicked", RoutingStrategy.Bubble, typeof(ColorRoutedEventHandler), typeof(ColorCanvas));

        public event ColorRoutedEventHandler ColorPicked
        {
            add => AddHandler(ColorPickedEvent, value);
            remove => RemoveHandler(ColorPickedEvent, value);
        }

        #endregion
    }
}
