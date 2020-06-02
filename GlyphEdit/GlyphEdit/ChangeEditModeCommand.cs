using GlyphEdit.Controls.DocumentView;

namespace GlyphEdit
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
