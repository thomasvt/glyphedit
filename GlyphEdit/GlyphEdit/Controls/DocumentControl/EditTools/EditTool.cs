using GlyphEdit.Controls.DocumentControl.Input;

namespace GlyphEdit.Controls.DocumentControl.EditTools
{
    internal abstract class EditTool
    {
        protected EditTool(EditMode editMode)
        {
            EditMode = editMode;
        }

        public EditMode EditMode { get; }

        protected internal virtual void OnKeyDown(object sender, KeyEventArgs args) { }
        protected internal virtual void OnKeyUp(object sender, KeyEventArgs args) { }
        protected internal virtual void OnMouseMove(object sender, MouseMoveEventArgs e) { }
        protected internal virtual void OnMouseLeftButtonDown(object sender, MouseEventArgs e) { }
        protected internal virtual void OnMouseLeftButtonUp(object sender, MouseEventArgs e) { }
    }
}