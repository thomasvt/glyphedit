using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
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

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            

            return base.ArrangeOverride(arrangeBounds);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            // get device dependent pixel size of control
            var source = PresentationSource.FromVisual(this);
            var transformToDevice = source.CompositionTarget.TransformToDevice;
            var actualSize = (Size)transformToDevice.Transform((Vector)this.RenderSize);

            // calculate pixels of color canvas
            var pixels = new byte[(int)actualSize.Width * (int)actualSize.Height * 4];
            

            var saturation = Saturation;

            var width = (int)actualSize.Width;
            var height = (int)actualSize.Height;
            var i = 0;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var luminance = 1f - (float)y / height;
                    var hue = (float)x / width;
                    var color = ColorUtils.FromHsl(hue, saturation, luminance);

                    pixels[i++] = color.B;
                    pixels[i++] = color.G;
                    pixels[i++] = color.R;
                    i++; // Bgr32 format ignores 4th byte
                }
            }

            if (_bitmap == null || (_bitmap.PixelWidth == width && _bitmap.PixelHeight == height))
                _bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null); // heavy ctor, reuse instance
            _bitmap.WritePixels(new Int32Rect(0, 0, (int)actualSize.Width, (int)actualSize.Height), pixels, (int)actualSize.Width * 4, 0);
            drawingContext.DrawImage(_bitmap, new Rect(RenderSize));
        }

        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
            "Saturation", typeof(float), typeof(ColorCanvas), new FrameworkPropertyMetadata(1f, FrameworkPropertyMetadataOptions.AffectsRender));

        private WriteableBitmap _bitmap;

        public float Saturation
        {
            get => (float)GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }
    }
}
