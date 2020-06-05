using System;
using System.Collections.Generic;

namespace GlyphEdit.Model
{
    public class Document
    {
        internal List<Layer> Layers { get; private set; }
        
        public Document(int width, int height)
        {
            Width = width;
            Height = height;
            Layers = new List<Layer> {new Layer(Width, Height)};

        }

        public readonly int Width;
        public readonly int Height;

        public bool IsInRange(VectorI documentVectorI)
        {
            var (x, y) = documentVectorI;
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        public ref DocumentElement GetElementRef(int layerIndex, VectorI vectorI)
        {
            if (!IsInRange(vectorI))
                throw new ArgumentOutOfRangeException($"({vectorI}) are not a valid position in the document. Use IsInRange() to test first.");

            return ref Layers[layerIndex].GetElementRef(vectorI);
        }

        public Layer GetLayer(int layerIndex)
        {
            if (layerIndex < 0 || layerIndex >= Layers.Count)
                throw new ArgumentOutOfRangeException($"LayerIndex {layerIndex} does not exist.");
            return Layers[layerIndex];
        }

        public int LayerCount => Layers.Count;

        /// <summary>
        /// Direct access for internal bulk purposes (persistence)
        /// </summary>
        internal void SetLayers(List<Layer> layers)
        {
            Layers = layers;
        }
    }
}
