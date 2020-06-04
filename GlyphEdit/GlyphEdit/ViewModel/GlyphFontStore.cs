using System;
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
                    try
                    {
                        var fontBitmap = FontBitmapLoader.Load(filename);
                        var width = int.Parse(match.Groups["width"].Value);
                        var height = int.Parse(match.Groups["height"].Value);
                        var fontName = match.Groups["name"].Value;
                        if (fontBitmap.PixelWidth / width != 16 || fontBitmap.PixelHeight / height != 16 ||
                            fontBitmap.PixelWidth % width != 0 || fontBitmap.PixelHeight % height != 0)
                        {
                            GlyphFonts.Add(GlyphFont.CreateInvalid(filename,
                                $"All fonts must fit exactly 16x16 characters: expected image of {16 * width}x{16 * height} but is {fontBitmap.PixelWidth}x{fontBitmap.PixelHeight}."));
                        }
                        else 
                        {
                            var glyphFont = GlyphFont.Create(filename, fontName, new Point(width, height), fontBitmap);
                            GlyphFonts.Add(glyphFont);
                        }
                    }
                    catch (Exception e)
                    {
                        GlyphFonts.Add(GlyphFont.CreateInvalid(filename, "Could not load font from file: " + e.Message));
                    }
                }
                else
                {
                    GlyphFonts.Add(GlyphFont.CreateInvalid(filename, "Filename must match pattern <name>_<charwidth>x<charheight>.png"));
                }
            }
            MessageBus.Publish(new GlyphFontListLoadedEvent(GlyphFonts.ToArray()));
        }
    }
}
