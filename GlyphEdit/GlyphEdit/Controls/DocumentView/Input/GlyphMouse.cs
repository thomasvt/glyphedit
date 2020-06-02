using System;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;

namespace GlyphEdit.Controls.DocumentView.Input
{
    public class GlyphMouse
    {
        private MouseState _previousMouseState;
        private readonly WpfMouse _mouse;

        public GlyphMouse(WpfGame wpfGame)
        {
            _mouse = new WpfMouse(wpfGame);
        }

        public void Update()
        {
            var mouseState = _mouse.GetState();

            if (mouseState.LeftButton != _previousMouseState.LeftButton)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                    LeftButtonDown?.Invoke(this, new MouseEventArgs(ref mouseState));
                else
                    LeftButtonUp?.Invoke(this, new MouseEventArgs(ref mouseState));
            }

            if (mouseState.MiddleButton != _previousMouseState.MiddleButton)
            {
                if (mouseState.MiddleButton == ButtonState.Pressed)
                    MiddleButtonDown?.Invoke(this, new MouseEventArgs(ref mouseState));
                else
                    MiddleButtonUp?.Invoke(this, new MouseEventArgs(ref mouseState));
            }

            if (mouseState.RightButton != _previousMouseState.RightButton)
            {
                if (mouseState.RightButton == ButtonState.Pressed)
                    RightButtonDown?.Invoke(this, new MouseEventArgs(ref mouseState));
                else
                    RightButtonUp?.Invoke(this, new MouseEventArgs(ref mouseState));
            }

            if (mouseState.Position != _previousMouseState.Position)
                Moved?.Invoke(this, new MouseMoveEventArgs(mouseState, _previousMouseState.Position));

            _previousMouseState = mouseState;
        }

        public event EventHandler<MouseEventArgs> LeftButtonDown;
        public event EventHandler<MouseEventArgs> LeftButtonUp;
        public event EventHandler<MouseEventArgs> MiddleButtonDown;
        public event EventHandler<MouseEventArgs> MiddleButtonUp;
        public event EventHandler<MouseEventArgs> RightButtonDown;
        public event EventHandler<MouseEventArgs> RightButtonUp;
        public event EventHandler<MouseMoveEventArgs> Moved;
    }
}
