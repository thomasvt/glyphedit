using System.IO;
using GlyphEdit.Controls.DocumentView.Input;
using GlyphEdit.Controls.DocumentView.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;

namespace GlyphEdit.Controls.DocumentView
{
    public class GlyphDocumentViewer : WpfGame
    {
        private IGraphicsDeviceService _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private Point _position;
        private GlyphMouse _mouse;
        private GlyphKeyboard _keyboard;
        private Camera _camera;
        private DocumentControl _document;
        private Renderer _renderer;

        protected override void Initialize()
        {
            // must be initialized. required by Content loading and rendering (will add itself to the Services)
            // note that MonoGame requires this to be initialized in the constructor, while WpfInterop requires it to
            // be called inside Initialize (before base.Initialize())
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);

            _mouse = new GlyphMouse(this);
            _keyboard = new GlyphKeyboard(this);

            // must be called after the WpfGraphicsDeviceService instance was created
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _texture = Texture2D.FromStream(GraphicsDevice, File.OpenRead("test.png"));
            _camera = new Camera(_mouse);

            _renderer = new Renderer();
            _renderer.Load(GraphicsDevice);

            _document = new DocumentControl();
            _document.Load(GraphicsDevice);

            
        }

        protected override void Update(GameTime time)
        {
            _mouse.Update();
        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(Colors.FromHex("222222"));
            _renderer.SetCameraPosition(_camera.Position);
            _document.Render(_renderer);
        }

        protected override void UnloadContent()
        {
            _renderer.Unload();
            _document.Unload();
        }
    }
}
