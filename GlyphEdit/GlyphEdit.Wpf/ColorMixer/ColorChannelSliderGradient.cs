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
                var color = Colors.White;
                var value = (float)x / width;
                switch (channel)
                {
                    //case ColorChannel.Red:
                    //    color = Color.FromRgb((byte)(x * 255), color.G, color.B);
                    //    break;
                    //case ColorChannel.Green:
                    //    color = Color.FromRgb(color.R, (byte)(x * 255), color.B);
                    //    break;
                    //case ColorChannel.Blue:
                    //    color = Color.FromRgb(color.R, color.G, (byte)(x * 255));
                    //    break;
                    case ColorChannel.Hue:
                        color = ColorUtils.FromHsl(value, Saturation, Luminance);
                        break;
                    case ColorChannel.Saturation:
                        color = ColorUtils.FromHsl(Hue, value, Luminance);
                        break;
                    case ColorChannel.Luminance:
                        color = ColorUtils.FromHsl(Hue, Saturation, value);
                        break;
                }

                pixels[i++] = color.R;
                pixels[i++] = color.G;
                pixels[i++] = color.B;
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
            "Hue", typeof(float), typeof(ColorChannelSliderGradient), new FrameworkPropertyMetadata(default(float))
            {
                PropertyChangedCallback = (o, args) => (o as ColorChannelSliderGradient).CheckForGradientRedraw(ColorChannel.Hue)
            });

        public float Hue
        {
            get => (float) GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }

        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
            "Saturation", typeof(float), typeof(ColorChannelSliderGradient), new FrameworkPropertyMetadata(default(float))
            {
                PropertyChangedCallback = (o, args) => (o as ColorChannelSliderGradient).CheckForGradientRedraw(ColorChannel.Saturation)
            });

        public float Saturation
        {
            get => (float) GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }

        public static readonly DependencyProperty LuminanceProperty = DependencyProperty.Register(
            "Luminance", typeof(float), typeof(ColorChannelSliderGradient), new FrameworkPropertyMetadata(default(float)) 
            {
                PropertyChangedCallback = (o, args) => (o as ColorChannelSliderGradient).CheckForGradientRedraw(ColorChannel.Luminance)
            });

        public float Luminance
        {
            get => (float) GetValue(LuminanceProperty);
            set => SetValue(LuminanceProperty, value);
        }


        public static readonly DependencyProperty ColorChannelProperty = DependencyProperty.Register(
            "ColorChannel", typeof(ColorChannel), typeof(ColorChannelSliderGradient), new FrameworkPropertyMetadata(default(ColorChannel), FrameworkPropertyMetadataOptions.AffectsRender) 
            {
                PropertyChangedCallback = (o, args) => (o as ColorChannelSliderGradient).UpdateGradient()
            });

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
