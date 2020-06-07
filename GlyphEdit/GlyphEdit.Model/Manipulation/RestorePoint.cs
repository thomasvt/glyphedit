using System;
using System.Collections.Generic;

namespace GlyphEdit.Model.Manipulation
{
    /// <summary>
    /// Stores an incremental state of one or more layers. Mainly used for undo/redo features.
    /// </summary>
    public class RestorePoint
    {
        private readonly Dictionary<Guid, DocumentElement[,]> _newState;
        private readonly Dictionary<Guid, DocumentElement[,]> _previousState;

        public RestorePoint()
        {
            _newState = new Dictionary<Guid, DocumentElement[,]>();
            _previousState = new Dictionary<Guid, DocumentElement[,]>();
        }

        internal void StorePreviousState(Layer layer)
        {
            if (_newState.ContainsKey(layer.Id)) // we could be frienldy and just store it again, but calling this twice might be an indication there's a bug.
                throw new Exception("Already stored that layer as prevousState earlier.");

            var copy = CopyCanvas(layer);
            _previousState.Add(layer.Id, copy);
        }

        internal void StoreNewState(Layer layer)
        {
            if (_newState.ContainsKey(layer.Id))
                throw new Exception("Already stored that layer as newState earlier.");

            var copy = CopyCanvas(layer);
            _newState.Add(layer.Id, copy);
        }

        private static DocumentElement[,] CopyCanvas(Layer layer)
        {
            var layerStateCopy = new DocumentElement[layer.Width, layer.Height];
            Array.Copy(layer.Elements, layerStateCopy, layer.Width * layer.Height);
            return layerStateCopy;
        }

        public void Redo(Document document)
        {
            Apply(document, _newState);
        }

        public void Undo(Document document)
        {
            Apply(document, _previousState);
        }

        private void Apply(Document document, Dictionary<Guid, DocumentElement[,]> changeset)
        {
            foreach (var pair in changeset)
            {
                var layer = document.GetLayer(pair.Key);
                Array.Copy(pair.Value, layer.Elements, layer.Width * layer.Height);
            }
        }

        public bool HasPreviousStateFor(Guid layerId)
        {
            return _previousState.ContainsKey(layerId);
        }

        public bool HasNewStateFor(Guid layerId)
        {
            return _newState.ContainsKey(layerId);
        }

        public IEnumerable<Guid> GetAllPreviousStateLayerIds()
        {
            return _previousState.Keys;
        }

        public void Clear()
        {
            _previousState.Clear();
            _newState.Clear();
        }
    }
}
