using System.Windows.Media;
using GlyphEdit.Model;

namespace GlyphEdit.Wpf.ColorGrid
{
    public class ColorPatch
    {
        public Color Color { get; internal set; }
        public VectorI GridLocation { get; set; }
        public object Tag { get; set; }

        public ColorPatch(Color color, VectorI gridLocation)
        {
            Color = color;
            GridLocation = gridLocation;
        }
    }
}
