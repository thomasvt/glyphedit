namespace GlyphEdit.Controls.DocumentView
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