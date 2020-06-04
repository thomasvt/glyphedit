using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Media;

namespace GlyphEdit.Models
{
    public struct GlyphColor
    {
        public readonly byte R;
        public readonly byte G;
        public readonly byte B;
        public readonly byte A;

        public GlyphColor(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static GlyphColor FromPacked(uint color)
        {
            var b = color >> 24;
            var g = (color >> 16) & 255;
            var r = (color >> 8) & 255;
            var a = color & 255;
            return new GlyphColor((byte)r, (byte)g, (byte)b, (byte)a);
        }

        public uint ToPacked()
        {
            return ((uint)B << 24) | ((uint)G << 16) | ((uint)R << 8) | A;
        }
    }
}
