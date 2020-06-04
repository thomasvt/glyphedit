using GlyphEdit.Controls.DocumentView.Rendering;
using GlyphEdit.Models;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentView
{
    public class DocumentRenderer
    {
        private readonly DocumentControl.DocumentControl _documentControl;
        private readonly Camera _camera;
        private readonly BackgroundRenderer _backgroundRenderer;
        private readonly LayerRenderer _layerRenderer;

        public DocumentRenderer(DocumentControl.DocumentControl documentControl, Camera camera)
        {
            _documentControl = documentControl;
            _camera = camera;
            _backgroundRenderer = new BackgroundRenderer();
            _layerRenderer = new LayerRenderer(documentControl);
        }

        public void Load(GraphicsDevice graphicsDevice)
        {
            _backgroundRenderer.Load(graphicsDevice, _camera);
        }

        public void Render(IRenderer renderer, Document document)
        {
            var glyphWidth = _documentControl.CurrentGlyphMapTexture.GlyphWidth;
            var glyphHeight = _documentControl.CurrentGlyphMapTexture.GlyphHeight;
            _backgroundRenderer.Render(renderer, (int)(document.Width * glyphWidth), (int)(document.Height * glyphHeight));
            _layerRenderer.Render(renderer, document.GetLayer(0));
        }

        public void Unload()
        {
            _layerRenderer.Unload();
            _backgroundRenderer.Unload();
        }
    }
}
