namespace GlyphEdit.Controls.DocumentControl.EditTools
{
    internal abstract class EditTool
    {
        protected EditTool(EditMode editMode)
        {
            EditMode = editMode;
        }

        public EditMode EditMode { get; }
    }
}