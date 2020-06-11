using System.Windows.Media;

namespace GlyphEdit.Wpf
{
    /// <summary>
    /// A color that tracks both HSL and RGB channel values and keeps them in sync.
    /// </summary>
    public class HslRgbColor
    {
        public readonly byte R, G, B;
        public readonly double H, S, L;

        public HslRgbColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
            ColorUtils.RgbToHsl(R, G, B, out var h, out var s, out var l);
            H = h;
            S = s;
            L = l;
        }

        public HslRgbColor(double h, double s, double l)
        {
            H = h;
            S = s;
            L = l;
            ColorUtils.HslToRgb(H, S, L, out var r, out var g, out var b);
            R = r;
            G = g;
            B = b;
        }

        public HslRgbColor ChangeR(byte r)
        {
            return new HslRgbColor(r, G, B);
        }

        public HslRgbColor ChangeG(byte g)
        {
            return new HslRgbColor(R, g, B);
        }

        public HslRgbColor ChangeB(byte b)
        {
            return new HslRgbColor(R, G, b);
        }

        public HslRgbColor ChangeH(double h)
        {
            return new HslRgbColor(h, S, L);
        }

        public HslRgbColor ChangeS(double s)
        {
            return new HslRgbColor(H, s, L);
        }

        public HslRgbColor ChangeL(double l)
        {
            return new HslRgbColor(H, S, l);
        }

        public Color ToWpfColor()
        {
            return Color.FromRgb(R, G, B);
        }
    }
}
