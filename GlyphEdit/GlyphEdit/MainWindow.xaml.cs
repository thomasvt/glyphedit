using System.Windows;
using GlyphEdit.Controls.DocumentView.Model;
using GlyphEdit.Messaging;
using Microsoft.Xna.Framework;

namespace GlyphEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MessageBus.Subscribe<NewDocumentCommand>(HandleNewDocumentCommand);
        }

        private void HandleNewDocumentCommand(NewDocumentCommand obj)
        {
            var document = new Document(10, 10);
            DocumentViewer.Camera.MoveTo(new Vector2(document.Width * 0.5f * 12, document.Height * 0.5f * 12));
            DocumentViewer.Document = document;
            DocumentViewer.ViewSettings.GlyphSize = 12;
        }
    }
}
