using GlyphEdit.Controls.DocumentView.Rendering;
using GlyphEdit.Model;

namespace GlyphEdit.Controls.DocumentView
{
    internal class LayerRenderer
    {
        private readonly DocumentControl.DocumentControl _documentControl;

        public LayerRenderer(DocumentControl.DocumentControl documentControl)
        {
            _documentControl = documentControl;
        }

        public void Render(IRenderer renderer, Layer layer)
        {
            renderer.DrawElements(_documentControl.CurrentGlyphMapTexture, layer.GetElements());
        }

        public void Unload()
        {
        }
    }
}
