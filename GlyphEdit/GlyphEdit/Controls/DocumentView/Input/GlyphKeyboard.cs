using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;

namespace GlyphEdit.Controls.DocumentView.Input
{
    public class GlyphKeyboard
    {
        private readonly WpfKeyboard _keyboard;
        private KeyboardState _previousKeyboardState;

        public GlyphKeyboard(WpfGame wpfGame)
        {
            _keyboard = new WpfKeyboard(wpfGame);
        }

        public void Update()
        {
            var keyboardState = _keyboard.GetState();

            _previousKeyboardState = keyboardState;
        }
    }
}
