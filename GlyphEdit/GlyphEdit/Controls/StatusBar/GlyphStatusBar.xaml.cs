using System.Windows.Controls;
using GlyphEdit.Messages;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;

namespace GlyphEdit.Controls.StatusBar
{
    /// <summary>
    /// Interaction logic for GlyphStatusBar.xaml
    /// </summary>
    public partial class GlyphStatusBar : UserControl
    {
        public GlyphStatusBar()
        {
            InitializeComponent();

            MessageBus.Subscribe<ZoomChangedEvent>(e =>
            {
                ZoomStateButton.Content = $"{e.Zoom * 100:0}%";
            });
        }
    }
}
