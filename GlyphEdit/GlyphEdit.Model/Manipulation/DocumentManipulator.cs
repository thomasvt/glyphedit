﻿using System;

namespace GlyphEdit.Model.Manipulation
{
    public class DocumentManipulator
    {
        private readonly Document _document;
        private readonly UndoRedoStack<RestorePoint> _undoStack;
        
        public DocumentManipulator(Document document)
        {
            _document = document;
            _undoStack = new UndoRedoStack<RestorePoint>();
            StoreFullRestorePoint();
        }

        private void StoreFullRestorePoint()
        {
            // stores the entire state of the document as a RestorePoint on the undoredo-stack
            var restorePoint = new RestorePoint();
            foreach (var layer in _document.Layers)
            {
                restorePoint.StoreLayerState(layer);
            }
            _undoStack.Add(restorePoint);
            UndoStackChanged?.Invoke(this, EventArgs.Empty);
        }

        public DocumentManipulationScope BeginManipulation()
        {
            return new DocumentManipulationScope(this, _document, _undoStack.Current());
        }

        internal void AddRestorePoint(RestorePoint restorePoint)
        {
            _undoStack.Add(restorePoint);
            UndoStackChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Undo()
        {
            var restorePoint = _undoStack.Undo();
            restorePoint.Apply(_document);
            UndoStackChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Redo()
        {
            var restorePoint = _undoStack.Redo();
            restorePoint.Apply(_document);
            UndoStackChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanUndo()
        {
            return _undoStack.CanUndo();
        }

        public bool CanRedo()
        {
            return _undoStack.CanRedo();
        }

        public event EventHandler UndoStackChanged;

        public void ResetUndoStack()
        {
            _undoStack.Clear();
            StoreFullRestorePoint();
        }
    }
}
