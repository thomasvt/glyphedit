using System.Diagnostics;
using GlyphEdit.Controls.DocumentControl;
using GlyphEdit.Controls.DocumentControl.EditTools;

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
