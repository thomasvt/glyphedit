using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

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

    [TemplatePart(Name = "PART_RedSlider", Type = typeof(ColorChannelSlider))]
    [TemplatePart(Name = "PART_GreenSlider", Type = typeof(ColorChannelSlider))]
    [TemplatePart(Name = "PART_BlueSlider", Type = typeof(ColorChannelSlider))]
    [TemplatePart(Name = "PART_RedTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_GreenTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_BlueTextBox", Type = typeof(TextBox))]

    [TemplatePart(Name = "PART_HexCodeTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_ColorCanvas", Type = typeof(ColorCanvas))]
    [TemplatePart(Name = "PART_Example", Type = typeof(Shape))]
    public class ColorMixer : Control
    {
        private ColorChannelSlider _hueSlider, _saturationSlider, _luminanceSlider, _redSlider, _greenSlider, _blueSlider;
        private TextBox _hueTextBox, _saturationTextBox, _luminanceTextBox, _hexCodeTextBox, _redTextBox, _greenTextBox, _blueTextBox;
        private ColorCanvas _colorCanvas;
        private Shape _sampleShape;

        private bool _isTemplateApplied;

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
            GetTemplateParts();

            // ensure all fields contain the initial values because Color was set before ApplyTemplate().
            UpdateAll();

            _hueTextBox.KeyDown += ConvertEnterToTab;
            _hueTextBox.KeyDown += ConvertEnterToTab;
            _hueTextBox.KeyDown += ConvertEnterToTab;

            _hueTextBox.KeyDown += ConvertEnterToTab;
            _hueTextBox.KeyDown += ConvertEnterToTab;
            _hueTextBox.KeyDown += ConvertEnterToTab;


            _hueTextBox.LostFocus += (sender, args) => { OnValueTextBoxLostFocus(_hueTextBox, ColorChannel.Hue, Color.H, 1, 359); };
            _saturationTextBox.LostFocus += (sender, args) => { OnValueTextBoxLostFocus(_saturationTextBox, ColorChannel.Saturation, Color.S, 100, 1); };
            _luminanceTextBox.LostFocus += (sender, args) => { OnValueTextBoxLostFocus(_luminanceTextBox, ColorChannel.Luminance, Color.L, 100, 1); };

            _redTextBox.LostFocus += (sender, args) => { OnValueTextBoxLostFocus(_redTextBox, ColorChannel.Red, Color.R, 1, 255); };
            _greenTextBox.LostFocus += (sender, args) => { OnValueTextBoxLostFocus(_greenTextBox, ColorChannel.Green, Color.G, 1, 255); };
            _blueTextBox.LostFocus += (sender, args) => { OnValueTextBoxLostFocus(_blueTextBox, ColorChannel.Blue, Color.B, 1, 255); };

            _hueSlider.ValueChanged += (sender, args) => {  if (!WasUserChange(sender)) return; ChangeColorChannelValue(ColorChannel.Hue, _hueSlider.Value); };
            _saturationSlider.ValueChanged += (sender, args) => { if (!WasUserChange(sender)) return; ChangeColorChannelValue(ColorChannel.Saturation, _saturationSlider.Value); };
            _luminanceSlider.ValueChanged += (sender, args) => { if (!WasUserChange(sender)) return; ChangeColorChannelValue(ColorChannel.Luminance, _luminanceSlider.Value); };

            _redSlider.ValueChanged += (sender, args) => { if (!WasUserChange(sender)) return; ChangeColorChannelValue(ColorChannel.Red, _redSlider.Value); };
            _greenSlider.ValueChanged += (sender, args) => { if (!WasUserChange(sender)) return; ChangeColorChannelValue(ColorChannel.Green, _greenSlider.Value); };
            _blueSlider.ValueChanged += (sender, args) => { if (!WasUserChange(sender)) return; ChangeColorChannelValue(ColorChannel.Blue, _blueSlider.Value); };

            _hexCodeTextBox.LostFocus += (sender, args) =>
            {
                if (ColorUtils.IsHexCode(_hexCodeTextBox.Text))
                {
                    var color = ColorUtils.FromHex(_hexCodeTextBox.Text);
                    Color = new HslRgbColor(color.R, color.G, color.B); 
                }
                else
                {
                    UpdateHexCodeField(); // revert
                }
            };

            _colorCanvas.ColorPicked += (sender, args) => Color = args.Color;

            _isTemplateApplied = true;
        }

        private void ConvertEnterToTab(object sender, KeyEventArgs e)
        {
            
        }

        private void GetTemplateParts()
        {
            _hueSlider = GetTemplateChild<ColorChannelSlider>("PART_HueSlider");
            _saturationSlider = GetTemplateChild<ColorChannelSlider>("PART_SaturationSlider");
            _luminanceSlider = GetTemplateChild<ColorChannelSlider>("PART_LuminanceSlider");

            _hueTextBox = GetTemplateChild<TextBox>("PART_HueTextBox");
            _saturationTextBox = GetTemplateChild<TextBox>("PART_SaturationTextBox");
            _luminanceTextBox = GetTemplateChild<TextBox>("PART_LuminanceTextBox");

            _redSlider = GetTemplateChild<ColorChannelSlider>("PART_RedSlider");
            _greenSlider = GetTemplateChild<ColorChannelSlider>("PART_GreenSlider");
            _blueSlider = GetTemplateChild<ColorChannelSlider>("PART_BlueSlider");

            _redTextBox = GetTemplateChild<TextBox>("PART_RedTextBox");
            _greenTextBox = GetTemplateChild<TextBox>("PART_GreenTextBox");
            _blueTextBox = GetTemplateChild<TextBox>("PART_BlueTextBox");

            _hexCodeTextBox = GetTemplateChild<TextBox>("PART_HexCodeTextBox");
            _colorCanvas = GetTemplateChild<ColorCanvas>("PART_ColorCanvas");
            _sampleShape = GetTemplateChild<Shape>("PART_Example");
        }

        private static bool WasUserChange(object sender)
        {
            // not knowing the difference in an event between user initiated change and code initiated change is one of the more annoying aspects of WPF: it causes cascades of property updates.
            // checking for focus is a fairly robust way to know a control event comes from the user.
            return (sender as UIElement).IsFocused;
        }

        private void UpdateAll()
        {
            UpdateSlider(_hueSlider, Color.H);
            UpdateValueField(_hueTextBox, Color.H, 1);

            UpdateSlider(_saturationSlider, Color.S);
            UpdateValueField(_saturationTextBox, Color.S, 100);

            UpdateSlider(_luminanceSlider, Color.L);
            UpdateValueField(_luminanceTextBox, Color.L, 100);

            UpdateSlider(_redSlider, Color.R);
            UpdateValueField(_redTextBox, Color.R, 1);

            UpdateSlider(_greenSlider, Color.G);
            UpdateValueField(_greenTextBox, Color.G, 1);

            UpdateSlider(_blueSlider, Color.B);
            UpdateValueField(_blueTextBox, Color.B, 1);

            UpdateHexCodeField();
            UpdateSliderGradients();

            _sampleShape.Fill = new SolidColorBrush(Color.ToWpfColor());
        }

        private void OnValueTextBoxLostFocus(TextBox textBox, ColorChannel channel, double fallbackValue, double textboxValueFactor, double maxValue)
        {
            if (!double.TryParse(textBox.Text, out var value) || value < 0 || value > maxValue)
            {
                textBox.Text = (fallbackValue * textboxValueFactor).ToString("0");
            }
            else
            {
                ChangeColorChannelValue(channel, value / textboxValueFactor);
            }
        }

        private void ChangeColorChannelValue(ColorChannel channel, double value)
        {
            switch (channel)
            {
                case ColorChannel.Red:
                    Color = Color.ChangeR((byte)value);
                    break;
                case ColorChannel.Green:
                    Color = Color.ChangeG((byte)value);
                    break;
                case ColorChannel.Blue:
                    Color = Color.ChangeB((byte)value);
                    break;
                case ColorChannel.Hue:
                    Color = Color.ChangeH(value);
                    break;
                case ColorChannel.Saturation:
                    Color = Color.ChangeS(value);
                    break;
                case ColorChannel.Luminance:
                    Color = Color.ChangeL(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(channel), channel, null);
            }
            UpdateAll();
        }

        private void UpdateSliderGradients()
        {
            _saturationSlider.Hue = Color.H;
            _saturationSlider.Luminance = Color.L;

            _luminanceSlider.Hue = Color.H;
            _luminanceSlider.Saturation = Color.S;
        }

        private void UpdateSlider(ColorChannelSlider slider, double value)
        {
            if (slider == null)
                return;
            if ((float) slider.Value != value)
                slider.Value = value;
        }

        private void UpdateValueField(TextBox textBox, double value, double formatFactor)
        {
            if (textBox != null)
                textBox.Text = (value * formatFactor).ToString("0");
        }

        private void UpdateHexCodeField()
        {
            var hexCode = Color.ToWpfColor().ToHexString(false);
            if (_hexCodeTextBox.Text != hexCode)
                _hexCodeTextBox.Text = hexCode;
        }

        public static readonly DependencyProperty ColorMixModeProperty = DependencyProperty.Register(
            "ColorMixMode", typeof(ColorMixMode), typeof(ColorMixer), new PropertyMetadata(ColorMixMode.HSL));

        public ColorMixMode ColorMixMode
        {
            get => (ColorMixMode) GetValue(ColorMixModeProperty);
            set => SetValue(ColorMixModeProperty, value);
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(HslRgbColor), typeof(ColorMixer), new FrameworkPropertyMetadata(new HslRgbColor(0, 0, 0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            {
                PropertyChangedCallback = (o, args) =>
                {
                    if (o is ColorMixer colorMixer)
                    {
                        if (colorMixer._isTemplateApplied)
                            colorMixer.UpdateAll();
                    }
                }
            });

        public HslRgbColor Color
        {
            get => (HslRgbColor) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
    }
}
