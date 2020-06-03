using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Model
{
    public class Document
    {
        private readonly List<Layer> _layers;

        public Document(int width, int height)
        {
            Width = width;
            Height = height;
            _layers = new List<Layer> {new Layer(Width, Height)};

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

            return ref _layers[layerIndex].GetElementRef(coords);
        }

        public Layer GetLayer(int layerIndex)
        {
            if (layerIndex < 0 || layerIndex >= _layers.Count)
                throw new ArgumentOutOfRangeException($"LayerIndex {layerIndex} does not exist.");
            return _layers[layerIndex];
        }

        public int LayerCount => _layers.Count;
    }
}
