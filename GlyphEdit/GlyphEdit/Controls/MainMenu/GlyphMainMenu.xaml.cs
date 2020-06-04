using System.Windows;
using System.Windows.Controls;
using GlyphEdit.Messages;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messaging;
using GlyphEdit.Persistence;

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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MessageBus.Publish(new SaveDocumentCommand());
        }
    }
}
