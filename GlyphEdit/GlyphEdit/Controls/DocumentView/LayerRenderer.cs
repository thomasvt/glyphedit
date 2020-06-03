using GlyphEdit.Controls.DocumentView.Rendering;
using GlyphEdit.Model;

namespace GlyphEdit.Controls.DocumentView
{
    internal class LayerRenderer
    {
        private readonly DocumentControl _documentControl;

        public LayerRenderer(DocumentControl documentControl)
        {
            _documentControl = documentControl;
        }

        public void Render(IRenderer renderer, Layer layer)
        {
            renderer.DrawElements(_documentControl.CurrentGlyphMapTexture, layer.Elements);
        }

        public void Unload()
        {
        }
    }
}
