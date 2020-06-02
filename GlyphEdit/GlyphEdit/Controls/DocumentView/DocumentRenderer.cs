﻿using GlyphEdit.Controls.DocumentView.Model;
using GlyphEdit.Controls.DocumentView.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentView
{
    public class DocumentRenderer
    {
        private readonly BackgroundRenderer _backgroundRenderer;
        private readonly LayerRenderer _layerRenderer;

        public DocumentRenderer()
        {
            _backgroundRenderer = new BackgroundRenderer();
            _layerRenderer = new LayerRenderer();
        }

        public void Load(GraphicsDevice graphicsDevice)
        {
            _backgroundRenderer.Load(graphicsDevice);
            _layerRenderer.Load(graphicsDevice);
        }

        public void Render(IRenderer renderer, Document document, DocumentViewSettings viewSettings)
        {
            var glyphRenderSize = GetGlyphRenderSize();
            _backgroundRenderer.Render(renderer, (int)(document.Width * glyphRenderSize.X), (int)(document.Height * glyphRenderSize.Y));
            _layerRenderer.Render(renderer, document.GetLayer(0), viewSettings);
        }

        public void Unload()
        {
            _layerRenderer.Unload();
            _backgroundRenderer.Unload();
        }

        public Vector2 GetGlyphRenderSize()
        {
            return _layerRenderer.GetGlyphRenderSize();
        }
    }
}
