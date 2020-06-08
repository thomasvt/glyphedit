using System.Windows.Media;

namespace GlyphEdit.Wpf.ColorGrid
{
    public class ColorPatch
    {
        public Color Color { get; }
        public int Column { get; set; }
        public int Row { get; set; }
        public object Tag { get; set; }

        public ColorPatch(Color color, int column, int row)
        {
            Color = color;
            Column = column;
            Row = row;
        }
    }
}
