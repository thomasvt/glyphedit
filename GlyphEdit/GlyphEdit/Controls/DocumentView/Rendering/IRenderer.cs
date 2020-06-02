using GlyphEdit.Controls.DocumentView.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentView.Rendering
{
    public interface IRenderer
    {
        void DrawQuad(Texture2D texture, Quad quad);
        void DrawElements(GlyphMapTexture glyphMapTexture, DocumentElement[,] elements, Vector2 glyphRenderSize);
    }
}