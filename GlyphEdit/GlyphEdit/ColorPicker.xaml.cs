using System.Windows;
using GlyphEdit.Wpf;

namespace GlyphEdit
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker
    {
        public ColorPicker()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(HslRgbColor), typeof(ColorPicker), new FrameworkPropertyMetadata(new HslRgbColor(0, 0, 0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public HslRgbColor Color
        {
            get => (HslRgbColor) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
