using System.Reflection;
using GlyphEdit.Controls.DocumentView.Rendering;
using GlyphEdit.Images;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentView
{
    internal class DocumentBackgroundControl : IGpuResource
    {
        private Texture2D _checkeredTexture;

        public DocumentBackgroundControl()
        {
            FillColor = Color.White;
        }
        
        public void Load(GraphicsDevice graphicsDevice)
        {
            _checkeredTexture = Texture2D.FromStream(graphicsDevice, Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(ImageMarker), "checkered.png"));
        }

        public void Render(IRenderer renderer)
        {
            renderer.DrawQuad(_checkeredTexture, new Quad(new Vector2(0, 0), new Vector2(WidthPx, HeightPx), new Vector2(0, 0), new Vector2((float)WidthPx / _checkeredTexture.Width, (float)HeightPx / _checkeredTexture.Height), Color.White));
        }

        public void Unload()
        {
            _checkeredTexture.Dispose();
        }

        public int WidthPx { get; set; }
        public int HeightPx { get; set; }
        public Color FillColor { get; set; }
    }
}
