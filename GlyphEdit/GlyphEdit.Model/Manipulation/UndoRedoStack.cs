using System;
using System.Collections.Generic;

namespace GlyphEdit.Model.Manipulation
{
    internal class UndoRedoStack<T>
    {
        private readonly List<T> _stack;
        private int _currentStateIndex;

        public UndoRedoStack()
        {
            _stack = new List<T>();
            _currentStateIndex = -1; // no states yet
        }

        /// <summary>
        /// Adds the next state to the stack. If there were other Redo-able states, these are removed.
        /// </summary>
        public void Add(T state)
        {
            _stack.RemoveRange(_currentStateIndex + 1, _stack.Count - _currentStateIndex - 1);
            _stack.Add(state);
            _currentStateIndex++;
        }

        public bool CanUndo()
        {
            return _currentStateIndex > 0;
        }

        /// <summary>
        /// Returns the current RestorePoint with which you can Undo its changes, and moves back on the stack one position.
        /// </summary>
        public T Undo()
        {
            if (CanUndo())
                return _stack[_currentStateIndex--];

            throw new InvalidOperationException("No Undo states available.");
        }

        /// <summary>
        /// Moves forward on the stack and returns the restorepoint with which you can Redo the changes.
        /// </summary>
        public T Redo()
        {
            if (CanRedo())
                return _stack[++_currentStateIndex];

            throw new InvalidOperationException("No Redo states available.");
        }

        public bool CanRedo()
        {
            return _currentStateIndex < _stack.Count - 1;
        }

        /// <summary>
        /// Returns the state on the stack at current position without changing that position.
        /// </summary>
        public T Current()
        {
            return _stack[_currentStateIndex];
        }

        public void Clear()
        {
            _stack.Clear();
            _currentStateIndex = -1;
        }
    }
}
