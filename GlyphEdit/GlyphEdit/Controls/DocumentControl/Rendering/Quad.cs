using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentControl.Rendering
{
    public struct Quad
    {
        public readonly VertexPositionColorTexture BottomLeft;
        public readonly VertexPositionColorTexture TopRight;
        public readonly VertexPositionColorTexture BottomRight;
        public readonly VertexPositionColorTexture TopLeft;

        [DebuggerStepThrough]
        public Quad(Vector2 topLeft, Vector2 bottomRight, Vector2 topLeftTexture, Vector2 bottomRightTexture, Color color)
        {
            TopLeft = new VertexPositionColorTexture(new Vector3(topLeft, 0), color, topLeftTexture);
            BottomRight = new VertexPositionColorTexture(new Vector3(bottomRight, 0), color, bottomRightTexture);
            TopRight = new VertexPositionColorTexture(new Vector3(bottomRight.X, topLeft.Y, 0), color, new Vector2(bottomRightTexture.X, topLeftTexture.Y));
            BottomLeft = new VertexPositionColorTexture(new Vector3(topLeft.X, bottomRight.Y, 0), color, new Vector2(topLeftTexture.X, bottomRightTexture.Y));
        }
    }
}
