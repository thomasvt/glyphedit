using System;
using System.Collections.Generic;

namespace GlyphEdit.Model.Manipulation
{
    /// <summary>
    /// Scope wihtin document changes (will) form a single reversable operation on the undo stack.
    /// </summary>
    public class DocumentManipulationScope
    {
        private readonly DocumentManipulator _documentManipulator;
        private readonly Document _document;
        private readonly RestorePoint _previousRestorePoint;
        private readonly List<Guid> _touchedLayerIds;

        internal DocumentManipulationScope(DocumentManipulator documentManipulator, Document document, RestorePoint previousRestorePoint)
        {
            _documentManipulator = documentManipulator;
            _document = document;
            _previousRestorePoint = previousRestorePoint;
            _touchedLayerIds = new List<Guid>();
        }

        public LayerEditAccess GetLayerEditAccess(Guid layerId)
        {
            var layer = _document.GetLayer(layerId);
            _touchedLayerIds.Add(layerId);
            return new LayerEditAccess(layer);
        }

        public void Commit()
        {
            var restorePoint = new RestorePoint();
            foreach (var layerId in _touchedLayerIds)
            {
                restorePoint.StoreLayerState(_document.GetLayer(layerId));
            }
            // add a restorepoint, for the currently committed changes.
            _documentManipulator.AddRestorePoint(restorePoint);
        }

        /// <summary>
        /// Undoes all changes done in this editscope. You may continue to use the scope after this, and draw something else.
        /// </summary>
        public void Revert()
        {
            _previousRestorePoint.Apply(_document);
        }
    }
}
