using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentView
{
    public interface IGpuResource
    {
        void Load(GraphicsDevice graphicsDevice);
        void Unload();
    }
}