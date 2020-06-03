using System;
using System.Collections.Generic;
using GlyphEdit.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentView.Rendering
{
    /// <summary>
    /// A special texture containing all glyphs of a ASCII art font.
    /// </summary>
    public class GlyphMapTexture
    : IDisposable
    {
        public readonly GlyphFont GlyphFont;
        public readonly Texture2D Texture;
        public readonly int ColumnCount;
        public readonly float GlyphWidth;
        public readonly float GlyphHeight;
        public readonly float GlyphWidthU;
        public readonly float GlyphHeightV;
        private Dictionary<int, UvRect> _glyphUvLookup;

        public GlyphMapTexture(GlyphFont glyphFont, Texture2D texture, int glyphWidth, int glyphHeight)
        {
            GlyphFont = glyphFont;
            Texture = texture;
            if (texture.Width % glyphWidth != 0)
                throw new ArgumentOutOfRangeException($"Texture width ({texture.Width}) is not a multitude of GlyphWidth ({glyphWidth}).");

            ColumnCount = texture.Width / glyphWidth;
            GlyphWidth = glyphWidth;
            GlyphHeight = glyphHeight;
            GlyphWidthU = GlyphWidth / texture.Width;
            GlyphHeightV = GlyphHeight / texture.Height;

            ConvertGrayValuesToAlpha(texture);
            BuildGlyphUVLookup(texture, glyphHeight);
        }

        private static void ConvertGrayValuesToAlpha(Texture2D texture)
        {
            var pixels = new Color[texture.Width * texture.Height];
            texture.GetData(pixels);
            for (var i = 0; i < pixels.Length; i++)
            {
                ref var pixel = ref pixels[i];
                var grayValue = (pixel.R + pixel.G + pixel.B) / 3;
                pixel = new Color(255, 255, 255, grayValue);
            }
            texture.SetData(pixels);
        }

        private void BuildGlyphUVLookup(Texture2D texture, int glyphHeight)
        {
            var rowCount = texture.Height / glyphHeight; // vertical mismatch with glyphHeight is ok
            var glyphCount = ColumnCount * rowCount;
            _glyphUvLookup = new Dictionary<int, UvRect>(glyphCount);
            for (var i = 0; i < glyphCount; i++)
            {
                _glyphUvLookup.Add(i, GetGlyphUVRect(i));
            }
        }

        public void Dispose()
        {
            Texture.Dispose();
        }

        private UvRect GetGlyphUVRect(int glyphIndex)
        {
            var row = glyphIndex / ColumnCount;
            var column = glyphIndex % ColumnCount;
            return new UvRect(column * GlyphWidthU, row * GlyphHeightV, column * GlyphWidthU + GlyphWidthU, row * GlyphHeightV + GlyphHeightV);
        }

        public UvRect GetUVRect(int glyphIndex)
        {
            return _glyphUvLookup[glyphIndex];
        }
    }
}
