using System.Windows.Media;
using GlyphEdit.Model;

namespace GlyphEdit.Controls
{
    // extensions on Glyphcolor for WPF visualization
    public static class GlyphColorExtensions
    {
        public static Color ToWpfColor(this GlyphColor glyphColor)
        {
            return Color.FromArgb(glyphColor.A, glyphColor.R, glyphColor.G, glyphColor.B);
        }
    }
}
