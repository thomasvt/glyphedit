using GlyphEdit.Model;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentControl.Rendering
{
    public interface IRenderer
    {
        void DrawQuad(Texture2D texture, Quad quad);
        void DrawElements(GlyphMapTexture glyphMapTexture, DocumentElement[,] elements);
    }
}