using GlyphEdit.Model;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.DocumentControl
{
    internal static class XnaColorExtensions
    {
        public static Color ToXnaColor(this GlyphColor color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        public static Color ToXnaColor(this System.Windows.Media.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
