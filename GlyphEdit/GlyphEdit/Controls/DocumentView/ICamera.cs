using System.Windows;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.DocumentView
{
    public interface ICamera
    {
        Vector2 Position { get; }
        void MoveTo(Vector2 position);
    }
}