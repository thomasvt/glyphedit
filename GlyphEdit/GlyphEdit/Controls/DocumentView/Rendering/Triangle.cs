using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentView.Rendering
{
    internal struct Triangle
    {
        public readonly VertexPositionColorTexture A;
        public readonly VertexPositionColorTexture B;
        public readonly VertexPositionColorTexture C;

        public Triangle(VertexPositionColorTexture a, VertexPositionColorTexture b, VertexPositionColorTexture c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}
