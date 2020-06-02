using System.Reflection;
using GlyphEdit.Controls.DocumentView.Rendering;
using GlyphEdit.Images;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentView
{
    internal class BackgroundRenderer
    {
        private Texture2D _checkeredTexture;

        public void Load(GraphicsDevice graphicsDevice)
        {
            _checkeredTexture = Texture2D.FromStream(graphicsDevice, Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(ImageMarker), "checkered.png"));
        }

        public void Render(IRenderer renderer, int width, int height)
        {
            renderer.DrawQuad(_checkeredTexture, new Quad(new Vector2(0, 0), new Vector2(width, height), new Vector2(0, 0), new Vector2((float)width / _checkeredTexture.Width, (float)height / _checkeredTexture.Height), Color.White));
        }

        public void Unload()
        {
            _checkeredTexture.Dispose();
        }

        public int GetMaxTriangleCount()
        {
            return 2;
        }
    }
}
