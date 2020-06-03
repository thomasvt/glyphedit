using System;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Model
{
    public class GlyphFont
    : IEquatable<GlyphFont>
    {
        public string FontName { get; set; }
        public string Filename { get; set; }
        public Point GlyphSize { get; set; }
        public BitmapSource BitmapSource { get; set; }
        public int GlyphCount { get; set; }

        public static bool operator ==(GlyphFont a, GlyphFont b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(GlyphFont a, GlyphFont b)
        {
            return !Equals(a, b);
        }

        public bool Equals(GlyphFont other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Filename == other.Filename;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GlyphFont) obj);
        }

        public override int GetHashCode()
        {
            return (Filename != null ? Filename.GetHashCode() : 0);
        }
    }
}
