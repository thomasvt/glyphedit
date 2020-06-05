using System;
using System.Collections.Generic;
using System.Linq;
using GlyphEdit.Model.Manipulation;

namespace GlyphEdit.Model
{
    public class Document
    {
        private readonly Dictionary<Guid, Layer> _layersById;
        internal List<Layer> Layers { get; }
        
        internal Document(int width, int height, List<Layer> layers) // direct ctor for Persistence logic
        {
            Width = width;
            Height = height;
            Layers = layers;
            _layersById = Layers.ToDictionary(l => l.Id);
        }

        public Document(int width, int height)
        : this(width, height, new List<Layer> {  new Layer(Guid.NewGuid(), width, height) })
        {
        }

        public bool IsInRange(VectorI coords)
        {
            var (x, y) = coords;
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }
        
        /// <summary>
        /// This allows direct access to the data inside layers. For user-manipulations that are undo-able etc, create a <see cref="DocumentManipulationScope"/> through an <see cref="DocumentManipulator"/>.
        /// </summary>
        public Layer GetLayer(int layerIndex)
        {
            if (layerIndex < 0 || layerIndex >= Layers.Count)
                throw new ArgumentOutOfRangeException($"LayerIndex {layerIndex} does not exist.");
            return Layers[layerIndex];
        }
        
        internal Layer GetLayer(Guid layerId)
        {
            if (!_layersById.TryGetValue(layerId, out var layer))
                throw new ArgumentOutOfRangeException(nameof(layerId));

            return layer;
        }

        public int LayerCount => Layers.Count;
        public readonly int Width;
        public readonly int Height;
    }
}
