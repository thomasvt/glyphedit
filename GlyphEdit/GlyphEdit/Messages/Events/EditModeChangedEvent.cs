using System.Diagnostics;
using GlyphEdit.Controls.DocumentView;

namespace GlyphEdit.Messages.Events
{
    public class EditModeChangedEvent
    {
        public readonly EditMode EditMode;

        [DebuggerStepThrough]
        public EditModeChangedEvent(EditMode editMode)
        {
            EditMode = editMode;
        }
    }
}
