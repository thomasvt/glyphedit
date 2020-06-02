using System.Windows;
using GlyphEdit.Controls.DocumentView.Model;
using GlyphEdit.Messaging;
using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;

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
            MessageBus.Subscribe<ChangeEditModeCommand>(HandleChangeEditModeCommand);
        }

        private void HandleChangeEditModeCommand(ChangeEditModeCommand obj)
        {
            DocumentViewer.ChangeEditMode(obj.EditMode);
        }

        private void HandleNewDocumentCommand(NewDocumentCommand obj)
        {
            var document = new Document(100, 100);
            DocumentViewer.Document = document;
            DocumentViewer.Camera.Reset();
        }
    }
}
