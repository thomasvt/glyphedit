using System;

namespace GlyphEdit.Model.Manipulation
{
    /// <summary>
    /// Scope wihtin document changes (will) form a single reversable operation on the undo stack.
    /// </summary>
    public class DocumentManipulationScope
    {
        private readonly DocumentManipulator _documentManipulator;
        private readonly Document _document;
        private readonly RestorePoint _restorePoint;

        internal DocumentManipulationScope(DocumentManipulator documentManipulator, Document document)
        {
            _documentManipulator = documentManipulator;
            _document = document;
            _restorePoint = new RestorePoint();
        }

        public LayerEditAccess GetLayerEditAccess(Guid layerId)
        {
            var layer = _document.GetLayer(layerId);
            if (!_restorePoint.HasPreviousStateFor(layerId))
                _restorePoint.StorePreviousState(layer);
            return new LayerEditAccess(layer);
        }

        public void Commit()
        {
            foreach (var layerId in _restorePoint.GetAllPreviousStateLayerIds())
            {
                _restorePoint.StoreNewState(_document.GetLayer(layerId));
            }
            // add a restorepoint, for the currently committed changes.
            _documentManipulator.AddRestorePoint(_restorePoint);
        }

        /// <summary>
        /// Undoes all changes done in this editscope. You may continue to use the scope after this, and draw something else.
        /// </summary>
        public void Revert()
        {
            _restorePoint.Undo(_document);
            _restorePoint.Clear();
        }
    }
}
