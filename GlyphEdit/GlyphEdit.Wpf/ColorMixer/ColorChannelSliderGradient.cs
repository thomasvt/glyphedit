using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GlyphEdit.Wpf.ColorMixer
{
    public class ColorChannelSliderGradient : Control
    {
        private WriteableBitmap _bitmap;
        private Size _size;

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            _size = arrangeBounds;
            UpdateGradient();
            return base.ArrangeOverride(arrangeBounds);
        }

        private void CheckForGradientRedraw(ColorChannel? trigger = null)
        {
            if (trigger == ColorChannel)
                return;

            if (_size.Width == 0)
                return;

            UpdateGradient();

            InvalidateVisual();
        }

        private void UpdateGradient()
        {
            if (_size.Width == 0)
                return;

            var pixels = new byte[(int)_size.Width * 4];

            var width = (int)_size.Width;
            var channel = ColorChannel;
            var i = 0;
            for (var x = 0; x < width; x++)
            {
                HslRgbColor color = null;
                var value = (double)x / width;
                switch (channel)
                {
                    case ColorChannel.Red:
                        color = new HslRgbColor((byte)(value * 255), Green, Blue);
                        break;
                    case ColorChannel.Green:
                        color = new HslRgbColor(Green, (byte)(value * 255), Blue);
                        break;
                    case ColorChannel.Blue:
                        color = new HslRgbColor(Green, Blue, (byte)(value * 255));
                        break;
                    case ColorChannel.Hue:
                        color = new HslRgbColor(value * 365d, Saturation, Luminance);
                        break;
                    case ColorChannel.Saturation:
                        color = new HslRgbColor(Hue, value, Luminance);
                        break;
                    case ColorChannel.Luminance:
                        color = new HslRgbColor(Hue, Saturation, value);
                        break;
                }

                pixels[i++] = color.B;
                pixels[i++] = color.G;
                pixels[i++] = color.R;
                i++;
            }

            if (_bitmap == null || width != _bitmap.PixelWidth)
            {
                _bitmap = new WriteableBitmap(width, 1, 96, 96, PixelFormats.Bgr32, null); // this is a heavy one, reuse as much as possible
            }
            _bitmap.WritePixels(new Int32Rect(0, 0, width, 1), pixels, width * 4, 0);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (this.RenderSize.Width == 0 || this.RenderSize.Height == 0)
                return;

            drawingContext.DrawImage(_bitmap, new Rect(RenderSize));
        }

        public static readonly DependencyProperty HueProperty = DependencyProperty.Register(
            "Hue", typeof(double), typeof(ColorChannelSliderGradient), new FrameworkPropertyMetadata(default(double))
            {
                PropertyChangedCallback = (o, args) => (o as ColorChannelSliderGradient).CheckForGradientRedraw(ColorChannel.Hue)
            });

        public double Hue
        {
            get => (double) GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }

        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
            "Saturation", typeof(double), typeof(ColorChannelSliderGradient), new FrameworkPropertyMetadata(default(double))
            {
                PropertyChangedCallback = (o, args) => (o as ColorChannelSliderGradient).CheckForGradientRedraw(ColorChannel.Saturation)
            });

        public double Saturation
        {
            get => (double) GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }

        public static readonly DependencyProperty LuminanceProperty = DependencyProperty.Register(
            "Luminance", typeof(double), typeof(ColorChannelSliderGradient), new FrameworkPropertyMetadata(default(double)) 
            {
                PropertyChangedCallback = (o, args) => (o as ColorChannelSliderGradient).CheckForGradientRedraw(ColorChannel.Luminance)
            });

        public double Luminance
        {
            get => (double) GetValue(LuminanceProperty);
            set => SetValue(LuminanceProperty, value);
        }


        public static readonly DependencyProperty ColorChannelProperty = DependencyProperty.Register(
            "ColorChannel", typeof(ColorChannel), typeof(ColorChannelSliderGradient), new FrameworkPropertyMetadata(default(ColorChannel), FrameworkPropertyMetadataOptions.AffectsRender) 
            {
                PropertyChangedCallback = (o, args) => (o as ColorChannelSliderGradient).UpdateGradient()
            });


        public static readonly DependencyProperty RedProperty = DependencyProperty.Register(
            "Red", typeof(byte), typeof(ColorChannelSliderGradient), new PropertyMetadata(default(byte)));

        public byte Red
        {
            get => (byte) GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }

        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register(
            "Green", typeof(byte), typeof(ColorChannelSliderGradient), new PropertyMetadata(default(byte)));

        public byte Green
        {
            get => (byte) GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }

        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register(
            "Blue", typeof(byte), typeof(ColorChannelSliderGradient), new PropertyMetadata(default(byte)));

        public byte Blue
        {
            get => (byte) GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }

        /// <summary>
        /// Used to define which channel is controlled by (and visualized in the background of) this slider.
        /// </summary>
        public ColorChannel ColorChannel
        {
            get => (ColorChannel) GetValue(ColorChannelProperty);
            set => SetValue(ColorChannelProperty, value);
        }
    }
}
