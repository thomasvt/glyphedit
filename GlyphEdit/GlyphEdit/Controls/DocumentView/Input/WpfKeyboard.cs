using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;

namespace GlyphEdit.Controls.DocumentView.Input
{
    public class WpfKeyboard
    {
        private readonly MonoGame.Framework.WpfInterop.Input.WpfKeyboard _keyboard;
        private KeyboardState _previousKeyboardState;

        public WpfKeyboard(WpfGame wpfGame)
        {
            _keyboard = new MonoGame.Framework.WpfInterop.Input.WpfKeyboard(wpfGame);
        }

        public void Update()
        {
            var keyboardState = _keyboard.GetState();

            _previousKeyboardState = keyboardState;
        }
    }
}
