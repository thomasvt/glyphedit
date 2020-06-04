using GlyphEdit.Models;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.DocumentView.Rendering
{
    internal static class GlyphColorExtensions
    {
        public static Color ToRenderColor(this GlyphColor color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
