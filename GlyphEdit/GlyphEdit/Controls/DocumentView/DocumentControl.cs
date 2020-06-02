using System.Runtime.CompilerServices;
using GlyphEdit.Controls.DocumentView.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentView
{
    public class DocumentControl : IGpuResource
    {
        private readonly DocumentBackgroundControl _background;

        public DocumentControl()
        {
            _background = new DocumentBackgroundControl();
        }

        public void Load(GraphicsDevice graphicsDevice)
        {
            _background.Load(graphicsDevice);
            _background.WidthPx = 100;
            _background.HeightPx = 100;
            _background.FillColor = Color.White;
        }

        public void Render(IRenderer renderer)
        {
            _background.Render(renderer);
        }

        public void Unload()
        {
            _background.Unload();
        }
    }
}
