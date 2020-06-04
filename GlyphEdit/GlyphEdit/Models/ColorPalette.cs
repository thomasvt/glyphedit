using System;

namespace GlyphEdit.Models
{
    public class ColorPalette
    : IEquatable<ColorPalette>
    {
        public string Name { get; private set; }
        public int ColumnCount { get; private set; }
        public int RowCount { get; private set; }
        public GlyphColor[,] Colors { get; private set; }
        public bool IsValid { get; private set; }
        public string Error { get; private set; }

        public static ColorPalette Create(string name, GlyphColor[,] colors)
        {
            return new ColorPalette
            {
                Name = name,
                ColumnCount = colors.GetLength(0),
                RowCount = colors.GetLength(1),
                Colors = colors,
                IsValid = true
            };  
        }

        public static ColorPalette CreateInvalid(string filename, string error)
        {
            return new ColorPalette
            {
                Name = filename,
                ColumnCount = 0,
                RowCount = 0,
                Colors = null,
                IsValid = false,
                Error = error
            };
        }

        public static bool operator ==(ColorPalette a, ColorPalette b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(ColorPalette a, ColorPalette b)
        {
            return !Equals(a, b);
        }

        public bool Equals(ColorPalette other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ColorPalette) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}
