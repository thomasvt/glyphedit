using System.Windows;
using System.Windows.Controls;
using GlyphEdit.Messages;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messaging;

namespace GlyphEdit.Controls.MainMenu
{
    /// <summary>
    /// Interaction logic for GlyphMainMenu.xaml
    /// </summary>
    public partial class GlyphMainMenu : UserControl
    {
        public GlyphMainMenu()
        {
            InitializeComponent();
        }

        private void NewDocument_Click(object sender, RoutedEventArgs e)
        {
            MessageBus.Publish(new NewDocumentCommand());
        }
    }
}
