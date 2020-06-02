using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentView.Rendering
{
    public interface IRenderer
    {
        void DrawQuad(Texture2D texture, Quad quad);
    }
}