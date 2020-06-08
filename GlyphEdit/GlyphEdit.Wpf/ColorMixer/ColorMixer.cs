using System;
using System.Windows;
using System.Windows.Controls;

namespace GlyphEdit.Wpf.ColorMixer
{
    /// <summary>
    /// A typical RGB and HSL color mixer.
    /// </summary>
    [TemplatePart(Name = "PART_HueSlider", Type = typeof(ColorChannelSlider))]
    [TemplatePart(Name = "PART_SaturationSlider", Type = typeof(ColorChannelSlider))]
    [TemplatePart(Name = "PART_LuminanceSlider", Type = typeof(ColorChannelSlider))]
    [TemplatePart(Name = "PART_HueTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_SaturationTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_LuminanceTextBox", Type = typeof(TextBox))]
    public class ColorMixer : Control
    {
        private ColorChannelSlider _hueSlider, _saturationSlider, _luminanceSlider;
        private TextBox _hueTextBox, _saturationTextBox, _luminanceTextBox;

        static ColorMixer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorMixer), new FrameworkPropertyMetadata(typeof(ColorMixer)));
        }

        private T GetTemplateChild<T>(string name) where T : class
        {
            if (!(GetTemplateChild(name) is T child))
                throw new Exception($"Template part \"{name}\" not found or is not a {typeof(T).Name}.");
            return child;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _hueSlider = GetTemplateChild<ColorChannelSlider>("PART_HueSlider") ;
            _saturationSlider = GetTemplateChild<ColorChannelSlider>("PART_SaturationSlider");
            _luminanceSlider = GetTemplateChild<ColorChannelSlider>("PART_LuminanceSlider");

            _hueTextBox = GetTemplateChild<TextBox>("PART_HueTextBox");
            _saturationTextBox = GetTemplateChild<TextBox>("PART_SaturationTextBox");
            _luminanceTextBox = GetTemplateChild<TextBox>("PART_LuminanceTextBox");

            UpdateHue(Hue);
            UpdateSaturation(Saturation);
            UpdateLuminance(Luminance);

            _hueTextBox.LostFocus += (sender, args) =>
            {
                if (!int.TryParse(_hueTextBox.Text, out var hue))
                {
                    _hueTextBox.Text = ((int) (Hue * byte.MaxValue)).ToString();
                }
                else
                {
                    UpdateHue((float)hue / byte.MaxValue);
                }
            };
            _saturationTextBox.LostFocus += (sender, args) =>
            {
                if (!int.TryParse(_saturationTextBox.Text, out var saturation))
                {
                    _saturationTextBox.Text = ((int)(Saturation * byte.MaxValue)).ToString();
                }
                else
                {
                    UpdateHue((float)saturation / byte.MaxValue);
                }
            };
            _luminanceTextBox.LostFocus += (sender, args) =>
            {
                if (!int.TryParse(_luminanceTextBox.Text, out var luminance))
                {
                    _luminanceTextBox.Text = ((int)(Luminance * byte.MaxValue)).ToString();
                }
                else
                {
                    UpdateHue((float)luminance / byte.MaxValue);
                }
            };

            _hueSlider.ValueChanged += (sender, args) => 
            { 
                UpdateHue((float)_hueSlider.Value);
            };
            _saturationSlider.ValueChanged += (sender, args) =>
            {
                UpdateSaturation((float) _saturationSlider.Value);
            };
            _luminanceSlider.ValueChanged += (sender, args) =>
            {
                UpdateLuminance((float) _luminanceSlider.Value);
            };
        }

        private void UpdateHue(float hue)
        {
            if (_hueSlider != null)
                _hueSlider.Value = hue;
            if (_hueTextBox != null)
                _hueTextBox.Text = ToByteText(hue);

            if (hue == Hue)
                return; // prevent infinite update loops
            Hue = hue;
        }

        private void UpdateSaturation(float saturation)
        {
            if (_saturationSlider != null)
                _saturationSlider.Value = saturation;
            if (_saturationTextBox != null)
                _saturationTextBox.Text = ToByteText(saturation);

            if (saturation == Saturation)
                return; // prevent infinite update loops
            Saturation = saturation;
        }

        private void UpdateLuminance(float luminance)
        {
            

            if (_luminanceSlider != null)
                _luminanceSlider.Value = luminance;
            if (_luminanceTextBox != null)
                _luminanceTextBox.Text = ToByteText(luminance);

            if (luminance == Luminance)
                return; // prevent infinite update loops
            Luminance = luminance;
        }

        private string ToByteText(float value)
        {
            return ((int)(value * byte.MaxValue)).ToString();
        }

        public static readonly DependencyProperty ColorMixModeProperty = DependencyProperty.Register(
            "ColorMixMode", typeof(ColorMixMode), typeof(ColorMixer), new PropertyMetadata(ColorMixMode.HSL));

        public ColorMixMode ColorMixMode
        {
            get => (ColorMixMode) GetValue(ColorMixModeProperty);
            set => SetValue(ColorMixModeProperty, value);
        }

        public static readonly DependencyProperty HueProperty = DependencyProperty.Register(
            "Hue", typeof(float), typeof(ColorMixer), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            {
                PropertyChangedCallback = (o, args) =>
                {
                    var colorMixer = o as ColorMixer;
                    var hue = (float)args.NewValue;
                    colorMixer.UpdateHue(hue);
                }
            });

        public float Hue
        {
            get => (float) GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }

        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
            "Saturation", typeof(float), typeof(ColorMixer), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            {
                PropertyChangedCallback = (o, args) =>
                {
                    var colorMixer = o as ColorMixer;
                    var saturation = (float)args.NewValue;
                    colorMixer.UpdateSaturation(saturation);
                }
            });

        public float Saturation
        {
            get => (float) GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }

        public static readonly DependencyProperty LuminanceProperty = DependencyProperty.Register(
            "Luminance", typeof(float), typeof(ColorMixer), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            {
                PropertyChangedCallback = (o, args) =>
                {
                    var colorMixer = o as ColorMixer;
                    var luminance = (float)args.NewValue;
                    colorMixer.UpdateLuminance(luminance);
                }
            });

        public float Luminance
        {
            get => (float) GetValue(LuminanceProperty);
            set => SetValue(LuminanceProperty, value);
        }
    }
}
