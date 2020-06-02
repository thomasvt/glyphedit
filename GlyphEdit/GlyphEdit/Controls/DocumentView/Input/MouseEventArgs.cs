using System;
using Microsoft.Xna.Framework.Input;

namespace GlyphEdit.Controls.DocumentView.Input
{
    public class MouseEventArgs
    : EventArgs
    {
        public MouseEventArgs(ref MouseState mouseState)
        {
            MouseState = mouseState;
        }

        public MouseState MouseState { get; }
    }
}
