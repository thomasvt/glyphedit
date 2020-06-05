using System;
using System.Collections.Generic;

namespace GlyphEdit.Model.Manipulation
{
    /// <summary>
    /// Stores an incremental state of one or more layers. Mainly used for undo/redo features.
    /// </summary>
    public class RestorePoint
    {
        private readonly Dictionary<Guid, DocumentElement[,]> _layerStates;

        public RestorePoint()
        {
            _layerStates = new Dictionary<Guid, DocumentElement[,]>();
        }

        public bool ContainsLayer(Guid layerId)
        {
            return _layerStates.ContainsKey(layerId);
        }

        public void StoreLayerState(Layer layer)
        {
            if (_layerStates.ContainsKey(layer.Id))
                throw new Exception("Already stored that layer earlier.");

            var layerStateCopy = new DocumentElement[layer.Width, layer.Height];
            Array.Copy(layer.Elements, layerStateCopy, layer.Width * layer.Height);
            _layerStates.Add(layer.Id, layerStateCopy);
        }

        public void Apply(Document document)
        {
            foreach (var pair in _layerStates)
            {
                var layer = document.GetLayer(pair.Key);
                Array.Copy(pair.Value, layer.Elements, layer.Width * layer.Height);
            }
        }
    }
}
