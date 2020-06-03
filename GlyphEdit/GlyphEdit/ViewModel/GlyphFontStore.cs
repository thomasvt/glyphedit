using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using GlyphEdit.Model;
using Microsoft.Xna.Framework;

namespace GlyphEdit.ViewModel
{
    public class GlyphFontStore
    {
        private static readonly Regex FontFileRegex = new Regex("(?<name>.*)_(?<width>\\d+)x(?<height>\\d+)$");
        public List<GlyphFont> GlyphFonts { get; private set; }

        public void Initialize()
        {
            GlyphFonts = new List<GlyphFont>();
            foreach (var file in Directory.GetFiles("Fonts", "*.png"))
            {
                var filename = Path.GetFullPath(file);
                var match = FontFileRegex.Match(Path.GetFileNameWithoutExtension(filename));
                if (match.Success)
                {
                    var width = int.Parse(match.Groups["width"].Value);
                    var height = int.Parse(match.Groups["height"].Value);
                    var glyphFont = new GlyphFont
                    {
                        GlyphSize = new Point(width, height),
                        FontName = match.Groups["name"].Value,
                        Filename = filename,
                        BitmapSource = FontBitmapLoader.Load(filename),
                        GlyphCount = width * height
                    };
                    GlyphFonts.Add(glyphFont);
                }
            }
        }
    }
}
