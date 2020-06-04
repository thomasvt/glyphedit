using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Models
{
    public class Document
    {
        internal readonly List<Layer> Layers;
        
        public Document(int width, int height)
        {
            Width = width;
            Height = height;
            Layers = new List<Layer> {new Layer(Width, Height)};

        }

        public readonly int Width;
        public readonly int Height;

        public bool IsInRange(Point documentCoords)
        {
            var (x, y) = documentCoords;
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        internal ref DocumentElement GetElementRef(int layerIndex, Point coords)
        {
            if (!IsInRange(coords))
                throw new ArgumentOutOfRangeException($"({coords}) are not a valid position in the document. Use IsInRange() to test first.");

            return ref Layers[layerIndex].GetElementRef(coords);
        }

        public Layer GetLayer(int layerIndex)
        {
            if (layerIndex < 0 || layerIndex >= Layers.Count)
                throw new ArgumentOutOfRangeException($"LayerIndex {layerIndex} does not exist.");
            return Layers[layerIndex];
        }

        public int LayerCount => Layers.Count;
    }
}
