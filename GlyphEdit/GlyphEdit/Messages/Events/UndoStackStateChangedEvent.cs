namespace GlyphEdit.Messages.Events
{
    public class UndoStackStateChangedEvent
    {
        public bool CanUndo { get; }
        public bool CanRedo { get; }

        public UndoStackStateChangedEvent(bool canUndo, bool canRedo)
        {
            CanUndo = canUndo;
            CanRedo = canRedo;
        }
    }
}
