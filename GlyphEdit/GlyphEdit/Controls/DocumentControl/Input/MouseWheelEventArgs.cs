using System;
using Microsoft.Xna.Framework.Input;

namespace GlyphEdit.Controls.DocumentView.Input
{
    public class MouseWheelEventArgs
    : EventArgs
    {
        public readonly MouseState MouseState;
        public readonly int WheelValueChange;

        public MouseWheelEventArgs(MouseState mouseState, int wheelValueChange)
        {
            MouseState = mouseState;
            WheelValueChange = wheelValueChange;
        }
    }
}
