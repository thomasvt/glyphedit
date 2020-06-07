using System.Windows.Controls;
using GlyphEdit.Controls.DocumentControl.EditTools;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;

namespace GlyphEdit.Controls.ControlBar
{
    /// <summary>
    /// Interaction logic for GlyphControlBar.xaml
    /// </summary>
    public partial class GlyphControlBar : UserControl
    {
        public GlyphControlBar()
        {
            InitializeComponent();

            MessageBus.Subscribe<EditModeChangedEvent>(e => SwitchEditMode(e.EditMode));
        }

        private void SwitchEditMode(EditMode editMode)
        {
            ToolLabel.Content = editMode.ToString();
        }
    }
}
