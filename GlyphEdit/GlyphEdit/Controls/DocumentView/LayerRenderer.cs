using System.IO;
using GlyphEdit.Controls.DocumentView.Rendering;
using GlyphEdit.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentView
{
    internal class LayerRenderer
    {
        private GlyphMapTexture _glyphMapTexture;
        
        public void Load(GraphicsDevice graphicsDevice)
        {
            var texture = Texture2D.FromStream(graphicsDevice, File.OpenRead("Fonts\\cp437_20x20.png"));
            _glyphMapTexture = new GlyphMapTexture(texture, 20, 20);
        }

        public void Render(IRenderer renderer, Layer layer, DocumentViewSettings viewSettings)
        {
            renderer.DrawElements(_glyphMapTexture, layer.Elements, GetGlyphRenderSize());
        }

        public void Unload()
        {
            _glyphMapTexture.Dispose();
        }

        public Vector2 GetGlyphRenderSize()
        {
            return new Vector2(_glyphMapTexture.GlyphWidth, _glyphMapTexture.GlyphHeight);
        }
    }
}
