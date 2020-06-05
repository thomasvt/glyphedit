namespace GlyphEdit.Model
{
    public class Layer
    {
        internal readonly DocumentElement[,] Elements;

        internal Layer(int width, int height)
        {
            Elements = new DocumentElement[width, height];
        }

        public ref DocumentElement GetElementRef(VectorI vectorI)
        {
            var (x, y) = vectorI;
            return ref Elements[x, y];
        }

        public DocumentElement[,] GetElements()
        {
            return Elements;
        }
    }
}
