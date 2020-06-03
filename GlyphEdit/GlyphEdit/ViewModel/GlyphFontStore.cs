using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GlyphEdit.Messages;
using GlyphEdit.Messaging;
using GlyphEdit.Model;
using Microsoft.Xna.Framework;

namespace GlyphEdit.ViewModel
{
    public class GlyphFontStore
    {
        private static readonly Regex FontFileRegex = new Regex("(?<name>.*)_(?<width>\\d+)x(?<height>\\d+)$");
        public HashSet<GlyphFont> GlyphFonts { get; private set; }

        public void DetectGlyphFonts()
        {
            GlyphFonts = new HashSet<GlyphFont>();
            foreach (var file in Directory.GetFiles("Fonts", "*.png"))
            {
                var filename = Path.GetFullPath(file);
                var match = FontFileRegex.Match(Path.GetFileNameWithoutExtension(filename));
                if (match.Success)
                {
                    var width = int.Parse(match.Groups["width"].Value);
                    var height = int.Parse(match.Groups["height"].Value);
                    var fontBitmap = FontBitmapLoader.Load(filename);
                    var glyphFont = new GlyphFont
                    {
                        GlyphSize = new Point(width, height),
                        FontName = match.Groups["name"].Value,
                        Filename = filename,
                        BitmapSource = fontBitmap,
                        GlyphCount = (fontBitmap.PixelWidth / width) * (fontBitmap.PixelHeight / height)
                    };
                    GlyphFonts.Add(glyphFont);
                }
            }
            MessageBus.Publish(new GlyphFontListLoadedEvent(GlyphFonts.ToArray()));
        }
    }
}
