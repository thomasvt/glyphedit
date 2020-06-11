using System.Windows.Media;
using GlyphEdit.Model;

namespace GlyphEdit.Controls
{
    // extensions on Glyphcolor for WPF visualization
    public static class ColorExtensions
    {
        public static Color ToWpfColor(this Microsoft.Xna.Framework.Color xnaColor)
        {
            return Color.FromArgb(xnaColor.A, xnaColor.R, xnaColor.G, xnaColor.B);
        }

        public static Color ToWpfColor(this GlyphColor glyphColor)
        {
            return Color.FromArgb(glyphColor.A, glyphColor.R, glyphColor.G, glyphColor.B);
        }

        public static GlyphColor ToGlyphColor(this Color color)
        {
            return new GlyphColor(color.R, color.G, color.B, color.A);
        }
    }
}
