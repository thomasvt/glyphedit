using System.Windows.Controls;
using GlyphEdit.Messages;
using GlyphEdit.Messaging;

namespace GlyphEdit.Controls.PanelBar.GlyphPicker
{
    /// <summary>
    /// Interaction logic for GlyphPicker.xaml
    /// </summary>
    public partial class GlyphPicker : UserControl
    {
        public GlyphPicker()
        {
            InitializeComponent();

            MessageBus.Subscribe<GlyphFontChangedEvent>(e => {});
        }
    }
}
