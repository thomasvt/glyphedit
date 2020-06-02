using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GlyphEdit.Controls.DocumentView.Input
{
    public class MouseMoveEventArgs
    : EventArgs
    {
        public MouseState MouseState { get; }
        public Point PreviousPosition { get; }

        public MouseMoveEventArgs(MouseState mouseState, Point previousPosition)
        {
            MouseState = mouseState;
            PreviousPosition = previousPosition;
        }
    }
}
