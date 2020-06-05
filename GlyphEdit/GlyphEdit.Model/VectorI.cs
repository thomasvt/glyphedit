namespace GlyphEdit.Model
{
    public struct VectorI
    {
        public readonly int X;
        public readonly int Y;

        public VectorI(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }
    }
}
