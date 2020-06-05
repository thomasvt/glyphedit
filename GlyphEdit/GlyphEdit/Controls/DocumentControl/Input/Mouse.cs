using System;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;

namespace GlyphEdit.Controls.DocumentControl.Input
{
    public class Mouse
    {
        private MouseState _previousMouseState;
        private readonly WpfMouse _mouse;

        public Mouse(WpfGame wpfGame)
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
                MouseMove?.Invoke(this, new MouseMoveEventArgs(mouseState, _previousMouseState.Position));

            if (mouseState.ScrollWheelValue != _previousMouseState.ScrollWheelValue)
                MouseWheel?.Invoke(this, new MouseWheelEventArgs(mouseState, mouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue));

            _previousMouseState = mouseState;
        }

        public event EventHandler<MouseEventArgs> LeftButtonDown;
        public event EventHandler<MouseEventArgs> LeftButtonUp;
        public event EventHandler<MouseEventArgs> MiddleButtonDown;
        public event EventHandler<MouseEventArgs> MiddleButtonUp;
        public event EventHandler<MouseEventArgs> RightButtonDown;
        public event EventHandler<MouseEventArgs> RightButtonUp;
        public event EventHandler<MouseMoveEventArgs> MouseMove;
        public event EventHandler<MouseWheelEventArgs> MouseWheel;
    }
}
