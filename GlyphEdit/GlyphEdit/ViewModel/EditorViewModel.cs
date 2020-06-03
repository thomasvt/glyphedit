using GlyphEdit.Controls.DocumentView;
using GlyphEdit.Messages;
using GlyphEdit.Messaging;
using GlyphEdit.Model;

namespace GlyphEdit.ViewModel
{
    /// <summary>
    /// I don't use standard MVVM because I don't like bidirectional databinding anymore.
    /// Instead, we use a fully separated "viewmodel" singleton and communicate towards controls using the MessageBus. The data is in dedicated, separate Models.
    /// </summary>
    public class EditorViewModel
    {
        public void OnLoaded()
        {
            CreateNewDocument();
        }

        public void CreateNewDocument()
        {
            Document = new Document(50, 50);
            MessageBus.Publish(new DocumentOpenedEvent(Document));
        }

        public void ChangeEditMode(EditMode editMode)
        {
            if (editMode != EditMode)
            {
                EditMode = editMode;
                MessageBus.Publish(new EditModeChangedEvent(editMode));
            }
        }

        public Document Document { get; private set; }

        public EditMode EditMode { get; private set; }

        public static EditorViewModel Current = new EditorViewModel();
    }
}
