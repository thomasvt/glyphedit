using System;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;

namespace GlyphEdit.Controls.DocumentControl.Input
{
    public class Keyboard
    {
        private readonly WpfKeyboard _keyboard;
        private KeyboardState _previousKeyboardState;

        public Keyboard(WpfGame wpfGame)
        {
            _keyboard = new WpfKeyboard(wpfGame);
        }

        public void Update()
        {
            var keyboardState = _keyboard.GetState();

            var pressedKeys = keyboardState.GetPressedKeys().ToHashSet();
            var previousPressedKeys = _previousKeyboardState.GetPressedKeys().ToHashSet();

            if (KeyUp != null)
            {
                foreach (var key in previousPressedKeys)
                {
                    if (!pressedKeys.Contains(key))
                        KeyUp.Invoke(this, new KeyEventArgs(key));
                }
            }

            if (KeyDown != null)
            {
                foreach (var key in pressedKeys)
                {
                    if (!previousPressedKeys.Contains(key))
                        KeyDown.Invoke(this, new KeyEventArgs(key));
                }
            }

            _previousKeyboardState = keyboardState;
        }

        public event EventHandler<KeyEventArgs> KeyUp;
        public event EventHandler<KeyEventArgs> KeyDown;
    }
}
