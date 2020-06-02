namespace GlyphEdit.Controls.DocumentView.Model
{
    public class Document
    {
        private DocumentElement[,] _elements;

        public Document(int width, int height)
        {
            Width = width;
            Height = height;
            _elements = new DocumentElement[width, height];
        }

        public readonly int Width;
        public readonly int Height;
    }
}
