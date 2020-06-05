using System;

namespace GlyphEdit.Model
{
    public class Layer
    {
        public Guid Id;
        public readonly int Width;
        public readonly int Height;
        internal readonly DocumentElement[,] Elements;

        internal Layer(Guid id, int width, int height)
        {
            Id = id;
            Width = width;
            Height = height;
            Elements = new DocumentElement[width, height];
        }

        /// <summary>
        /// Provides direct access to the data for readfast -only purposes. There is no check that prevents you from writing. Use <see cref="DocumentEditScope"/>s for editing.
        /// </summary>
        public DocumentElement[,] GetElementsForReadOnly()
        {
            return Elements;
        }
    }
}
