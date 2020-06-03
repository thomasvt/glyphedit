using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Model
{
    public class GlyphFont
    {
        public string FontName { get; set; }
        public string Filename { get; set; }
        public Point GlyphSize { get; set; }
        public BitmapSource BitmapSource { get; set; }
        public int GlyphCount { get; set; }
    }
}
