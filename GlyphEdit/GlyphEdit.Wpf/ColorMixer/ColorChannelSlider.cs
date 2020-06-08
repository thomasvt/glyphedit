using System;
using System.Windows;
using System.Windows.Controls;

namespace GlyphEdit.Wpf.ColorMixer
{
    public class ColorChannelSlider : Slider
    {
        public ColorChannelSlider()
        {
            Minimum = 0;
            Maximum = 1;
            SmallChange = 0.01;
            LargeChange = 0.1;
        }

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
            "Hue", typeof(float), typeof(ColorChannelSlider), new FrameworkPropertyMetadata(default(float)));

        public float Hue
        {
            get => (float) GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }

        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
            "Saturation", typeof(float), typeof(ColorChannelSlider), new FrameworkPropertyMetadata(default(float)));

        public float Saturation
        {
            get => (float) GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }

        public static readonly DependencyProperty LuminanceProperty = DependencyProperty.Register(
            "Luminance", typeof(float), typeof(ColorChannelSlider), new FrameworkPropertyMetadata(default(float)));

        public float Luminance
        {
            get => (float) GetValue(LuminanceProperty);
            set => SetValue(LuminanceProperty, value);
        }
    }
}
