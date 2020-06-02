using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;

namespace GlyphEdit.Controls.DocumentView
{
    public class GlyphDocumentViewer : WpfGame
    {
        private IGraphicsDeviceService _graphicsDeviceManager;
        private WpfKeyboard _keyboard;
        private WpfMouse _mouse;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private Point _position;
        private KeyboardState _previousKeyboardState;
        private MouseState _previousMouseState;
        private Camera _camera;

        protected override void Initialize()
        {
            // must be initialized. required by Content loading and rendering (will add itself to the Services)
            // note that MonoGame requires this to be initialized in the constructor, while WpfInterop requires it to
            // be called inside Initialize (before base.Initialize())
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);

            // wpf and keyboard need reference to the host control in order to receive input
            // this means every WpfGame control will have it's own keyboard & mouse manager which will only react if the mouse is in the control
            _keyboard = new WpfKeyboard(this);
            _mouse = new WpfMouse(this);

            // must be called after the WpfGraphicsDeviceService instance was created
            base.Initialize();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _texture = Texture2D.FromStream(GraphicsDevice, File.OpenRead("test.png"));
            _camera = new Camera();
        }

        protected override void Update(GameTime time)
        {
            var mouseState = _mouse.GetState();
            var keyboardState = _keyboard.GetState();

            if (_previousMouseState.MiddleButton != mouseState.MiddleButton)
            {
                if (mouseState.MiddleButton == ButtonState.Pressed)
                    _camera.StartPan(mouseState.Position);
                else
                    _camera.EndPan();
            }
            else
            {
                if (_camera.IsPanning())
                    _camera.UpdatePan(mouseState.Position);
            }
            


            _previousMouseState = mouseState;
            _previousKeyboardState = keyboardState;
        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(Colors.FromHex("222222"));

            _spriteBatch.Begin();
            _spriteBatch.Draw(_texture, _position.ToVector2() - _camera.Position, Color.White);
            _spriteBatch.End();
        }
    }
}
