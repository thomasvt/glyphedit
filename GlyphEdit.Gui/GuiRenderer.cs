using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Gui
{
    internal class GuiRenderer
    : IDisposable
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly SpriteBatch _spriteBatch;
        private SpriteFont _font;

        public GuiRenderer(ContentManager contentManager, GraphicsDevice graphicsDevice, GuiTheme theme)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _font = contentManager.Load<SpriteFont>(theme.FontName);
        }

        public void BeginRender()
        {
            
        }

        public void EndRender()
        {
            
        }

        public void Dispose()
        {
            _spriteBatch.Dispose();
        }
    }
}
