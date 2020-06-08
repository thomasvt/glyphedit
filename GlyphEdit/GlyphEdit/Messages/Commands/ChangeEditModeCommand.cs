using GlyphEdit.Controls.DocumentControl.EditTools;

namespace GlyphEdit.Messages.Commands
{
    public class ChangeEditModeCommand
    {
        public EditMode EditMode { get; }

        public ChangeEditModeCommand(EditMode editMode)
        {
            EditMode = editMode;
        }
    }
}
