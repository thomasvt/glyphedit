using System.Windows.Media;

namespace GlyphEdit.Wpf.ColorGrid
{
    public class ColorPatch
    {
        public Color Color { get; set; }
        public PointI GridLocation { get; set; }

        public ColorPatch(Color color, PointI gridLocation)
        {
            Color = color;
            GridLocation = gridLocation;
        }
    }
}
