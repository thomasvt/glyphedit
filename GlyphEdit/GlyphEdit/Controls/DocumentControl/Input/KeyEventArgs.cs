using Microsoft.Xna.Framework.Input;

namespace GlyphEdit.Controls.DocumentControl.Input
{
    public class KeyEventArgs
    {
        public readonly Keys Key;

        public KeyEventArgs(Keys key)
        {
            Key = key;
        }
    }
}
