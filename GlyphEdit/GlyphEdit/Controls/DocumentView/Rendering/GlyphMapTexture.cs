using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentView.Rendering
{
    /// <summary>
    /// A special texture containing all glyphs of a ASCII art font.
    /// </summary>
    public class GlyphMapTexture
    : IDisposable
    {
        public readonly Texture2D Texture;
        public readonly int ColumnCount;
        public readonly float GlyphWidth;
        public readonly float GlyphHeight;
        public readonly float GlyphWidthU;
        public readonly float GlyphHeightV;
        private readonly Dictionary<int, UvRect> _glyphUvLookup;

        public GlyphMapTexture(Texture2D texture, int glyphWidth, int glyphHeight)
        {
            Texture = texture;
            if (texture.Width % glyphWidth != 0)
                throw new ArgumentOutOfRangeException($"Texture width ({texture.Width}) is not a multitude of GlyphWidth ({glyphWidth}).");
            ColumnCount = texture.Width / glyphWidth;
            GlyphWidth = glyphWidth;
            GlyphHeight = glyphHeight;
            GlyphWidthU = GlyphWidth / texture.Width;
            GlyphHeightV = GlyphHeight / texture.Height;
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
