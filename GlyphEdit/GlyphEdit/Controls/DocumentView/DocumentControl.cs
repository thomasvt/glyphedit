using GlyphEdit.Controls.DocumentView.Model;
using GlyphEdit.Controls.DocumentView.Rendering;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentView
{
    public class DocumentControl
    {
        private readonly DocumentBackgroundControl _background;

        public DocumentControl()
        {
            _background = new DocumentBackgroundControl();
        }

        public void Load(GraphicsDevice graphicsDevice)
        {
            _background.Load(graphicsDevice);
        }

        public void Render(IRenderer renderer)
        {
            if (Document == null)
                return;

            _background.WidthPx = Document.Width * ViewSettings.GlyphSize;
            _background.HeightPx = Document.Height * ViewSettings.GlyphSize;
            _background.Render(renderer);
        }

        public void Unload()
        {
            _background.Unload();
        }

        public Document Document { get; set; }
        public DocumentViewSettings ViewSettings { get; set; }
    }
}
