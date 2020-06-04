using GlyphEdit.Models;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.DocumentControl
{
    /// <summary>
    /// GlyphColor extensions for rendering with monogame.
    /// </summary>
    public static class GlyphColorExtensions
    {
        public static Color ToMonogameColor(this GlyphColor color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
