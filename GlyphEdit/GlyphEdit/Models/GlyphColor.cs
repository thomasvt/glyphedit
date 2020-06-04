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
    }
}
