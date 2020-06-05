using System;

namespace GlyphEdit.Model.Manipulation
{
    public class LayerEditAccess
    {
        private readonly Layer _layer;

        internal LayerEditAccess(Layer layer)
        {
            _layer = layer;
        }

        public ref DocumentElement GetElementRef(VectorI coords)
        {
            if (coords.X < 0 || coords.Y < 0 || coords.X >= _layer.Width || coords.Y >= _layer.Height)
                throw new ArgumentOutOfRangeException($"({coords}) are not a valid position in the document.");

            var (x, y) = coords;
            return ref _layer.Elements[x, y];
        }
    }
}
