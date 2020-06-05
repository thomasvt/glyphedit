using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GlyphEdit.Messages;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;
using Microsoft.Xna.Framework;

namespace GlyphEdit.ViewModels
{
    public class GlyphFontStore
    {
        private static readonly Regex FontFileRegex = new Regex("(?<name>.*)_(?<width>\\d+)x(?<height>\\d+)$");
        public HashSet<GlyphFontViewModel> GlyphFonts { get; private set; }

        public void DetectGlyphFonts()
        {
            GlyphFonts = new HashSet<GlyphFontViewModel>();
            foreach (var file in Directory.GetFiles("Fonts", "*.png"))
            {
                var filename = Path.GetFullPath(file);
                var match = FontFileRegex.Match(Path.GetFileNameWithoutExtension(filename));
                if (match.Success)
                {
                    try
                    {
                        var fontBitmap = BitmapUtils.LoadAndCleanGlyphFontBitmap(filename);
                        var width = int.Parse(match.Groups["width"].Value);
                        var height = int.Parse(match.Groups["height"].Value);
                        var fontName = match.Groups["name"].Value;
                        if (fontBitmap.PixelWidth / width != 16 || fontBitmap.PixelHeight / height != 16 ||
                            fontBitmap.PixelWidth % width != 0 || fontBitmap.PixelHeight % height != 0)
                        {
                            GlyphFonts.Add(GlyphFontViewModel.CreateInvalid(filename,
                                $"All fonts must fit exactly 16x16 characters: expected image of {16 * width}x{16 * height} but is {fontBitmap.PixelWidth}x{fontBitmap.PixelHeight}."));
                        }
                        else 
                        {
                            var glyphFont = GlyphFontViewModel.Create(filename, fontName, new Point(width, height), fontBitmap);
                            GlyphFonts.Add(glyphFont);
                        }
                    }
                    catch (Exception e)
                    {
                        GlyphFonts.Add(GlyphFontViewModel.CreateInvalid(filename, "Could not load font from file: " + e.Message));
                    }
                }
                else
                {
                    GlyphFonts.Add(GlyphFontViewModel.CreateInvalid(filename, "Filename must match pattern <name>_<charwidth>x<charheight>.png"));
                }
            }
            MessageBus.Publish(new GlyphFontListLoadedEvent(GlyphFonts.ToArray()));
        }
    }
}
