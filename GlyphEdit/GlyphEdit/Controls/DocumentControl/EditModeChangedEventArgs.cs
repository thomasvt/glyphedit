using System;
using GlyphEdit.Controls.DocumentControl.EditTools;

namespace GlyphEdit.Controls.DocumentControl
{
    public class EditModeChangedEventArgs
    : EventArgs
    {
        public EditMode EditMode { get; }

        public EditModeChangedEventArgs(EditMode editMode)
        {
            EditMode = editMode;
        }
    }
}
