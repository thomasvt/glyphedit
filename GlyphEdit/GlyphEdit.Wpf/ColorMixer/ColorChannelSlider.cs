using System.Windows;
using System.Windows.Controls;

namespace GlyphEdit.Wpf.ColorMixer
{
    public class ColorChannelSlider : Slider
    {
        static ColorChannelSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorChannelSlider), new FrameworkPropertyMetadata(typeof(ColorChannelSlider)));
        }

        public static readonly DependencyProperty ColorChannelProperty = DependencyProperty.Register(
            "ColorChannel", typeof(ColorChannel), typeof(ColorChannelSlider), new PropertyMetadata(default(ColorChannel)));

        /// <summary>
        /// The channel that this slider controls.
        /// </summary>
        public ColorChannel ColorChannel
        {
            get => (ColorChannel) GetValue(ColorChannelProperty);
            set => SetValue(ColorChannelProperty, value);
        }

        public static readonly DependencyProperty HueProperty = DependencyProperty.Register(
            "Hue", typeof(double), typeof(ColorChannelSlider), new FrameworkPropertyMetadata(default(double)));

        public double Hue
        {
            get => (double) GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }

        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
            "Saturation", typeof(double), typeof(ColorChannelSlider), new FrameworkPropertyMetadata(default(double)));

        public double Saturation
        {
            get => (double) GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }

        public static readonly DependencyProperty LuminanceProperty = DependencyProperty.Register(
            "Luminance", typeof(double), typeof(ColorChannelSlider), new FrameworkPropertyMetadata(default(double)));

        public double Luminance
        {
            get => (double) GetValue(LuminanceProperty);
            set => SetValue(LuminanceProperty, value);
        }
    }
}
