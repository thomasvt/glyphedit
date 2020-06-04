﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Point = Microsoft.Xna.Framework.Point;

namespace GlyphEdit.Models
{
    public class GlyphFont
    : IEquatable<GlyphFont>
    {
        public string FontName { get; private set; }
        public string Filename { get; private set; }
        public Point GlyphSize { get; private set; }
        public BitmapSource BitmapSource { get; private set; }
        public int GlyphCount { get; private set; }
        public bool IsValid { get; private set; }
        public string Error { get; private set; }
        public int ColumnCount => BitmapSource.PixelWidth / GlyphSize.X;
        public int RowCount => BitmapSource.PixelHeight / GlyphSize.Y;

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

        internal static GlyphFont Create(string filename, string fontName, Point glyphSize, BitmapSource bitmapSource)
        {
            return new GlyphFont
            {
                Filename = filename,
                FontName = fontName,
                GlyphSize = glyphSize,
                BitmapSource = bitmapSource,
                GlyphCount = (bitmapSource.PixelWidth / glyphSize.X) * (bitmapSource.PixelHeight / glyphSize.Y),
                IsValid = true
            };
        }

        internal static GlyphFont CreateInvalid(string filename, string error)
        {
            return new GlyphFont 
            {
                Filename = filename,
                FontName = Path.GetFileName(filename),
                IsValid = false,
                Error = error
            };
        }

        public Int32Rect GetGlyphCropRectangle(int glyphIndex)
        {
            var x = glyphIndex % ColumnCount;
            var y = glyphIndex / ColumnCount;
            return new Int32Rect(x * GlyphSize.X, y * GlyphSize.Y, GlyphSize.X, GlyphSize.Y);
        }
    }
}
