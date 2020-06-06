using System.Windows;
using System.Windows.Controls;

namespace GlyphEdit
{
    /// <summary>
    /// Interaction logic for NewDocumentDialog.xaml
    /// </summary>
    public partial class NewDocumentDialog
    {
        public int DocumentWidth { get; private set; }
        public int DocumentHeight { get; private set; }
        public bool EnableCompression { get; private set; }

        public NewDocumentDialog()
        {
            InitializeComponent();
            DocumentWidth = 80;
            DocumentHeight = 80;
            EnableCompression = true;
            WidthTextBox.Text = DocumentWidth.ToString();
            HeightTextBox.Text = DocumentHeight.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EnableCompression = EnableCompressionCheckBox.IsChecked.GetValueOrDefault(true);
            DialogResult = true;
            Close();
        }


        private void WidthTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (int.TryParse(textBox.Text, out var width))
                {
                    DocumentWidth = width;
                }
                else
                {
                    textBox.Text = DocumentWidth.ToString();
                }
            }
        }

        private void HeightTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (int.TryParse(textBox.Text, out var height))
                {
                    DocumentHeight = height;
                }
                else
                {
                    textBox.Text = DocumentHeight.ToString();
                }
            }
        }
    }
}
