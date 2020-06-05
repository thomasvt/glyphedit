using System.Reflection;
using GlyphEdit.Controls.DocumentControl.Rendering;
using GlyphEdit.Images;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentControl
{
    internal class BackgroundRenderer
    {
        private Texture2D _checkeredTexture;
        private Camera _camera;

        public void Load(GraphicsDevice graphicsDevice, Camera camera)
        {
            _camera = camera;
            _checkeredTexture = Texture2D.FromStream(graphicsDevice, Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(ImageMarker), "checkered.png"));
        }

        public void Render(IRenderer renderer, int width, int height)
        {
            var checkersOffsetX = -_camera.Position.X / _checkeredTexture.Width * _camera.Zoom;
            var checkersOffsetY = -_camera.Position.Y / _checkeredTexture.Height * _camera.Zoom;
            var offset = new Vector2(checkersOffsetX, checkersOffsetY);
            renderer.DrawQuad(_checkeredTexture, new Quad(new Vector2(0, 0), new Vector2(width, height), offset, new Vector2((float)width / _checkeredTexture.Width, (float)height / _checkeredTexture.Height) * _camera.Zoom + offset, Color.White));
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
