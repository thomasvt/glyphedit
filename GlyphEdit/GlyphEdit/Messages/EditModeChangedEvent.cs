using GlyphEdit.Controls.DocumentView;

namespace GlyphEdit.Messages
{
    public class EditModeChangedEvent
    {
        public readonly EditMode EditMode;

        public EditModeChangedEvent(EditMode editMode)
        {
            EditMode = editMode;
        }
    }
}
