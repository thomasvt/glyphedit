using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace GlyphEdit.Toolbar
{
    /// <summary>
    /// Interaction logic for GlyphToolbar.xaml
    /// </summary>
    public partial class GlyphToolbar : UserControl
    {
        public GlyphToolbar()
        {
            InitializeComponent();

            ToolButtons = new[]
            {
                PencilToolButton,
                EraserToolButton
            };
        }

        private void Toolbutton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var toolButton in ToolButtons)
            {
                toolButton.IsChecked = (toolButton == sender);
            }
        }

        public ToggleButton[] ToolButtons { get; set; }
    }
}
