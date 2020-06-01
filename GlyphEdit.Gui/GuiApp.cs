using System;
using GlyphEdit.Gui.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Gui
{
    internal class GuiApp : Game
    {
        private GuiRenderer _renderer;

        private readonly Form _mainForm;
        private readonly GuiTheme _theme;
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        public GuiApp(Form mainForm, GuiTheme theme)
        {
            _mainForm = mainForm;
            _theme = theme;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += WindowOnClientSizeChanged;
            Window.IsBorderless = false;
            IsMouseVisible = true;

            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        private void WindowOnClientSizeChanged(object sender, EventArgs e)
        {
            _graphicsDeviceManager.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight = Window.ClientBounds.Height;
            _graphicsDeviceManager.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _renderer = new GuiRenderer(Content, GraphicsDevice, _theme);
        }

        protected override void Update(GameTime gameTime)
        {
        }

        protected override void Draw(GameTime gameTime)
        {
        }

        protected override void UnloadContent()
        {
            _renderer.Dispose();
        }
    }
}
