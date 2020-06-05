using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GlyphEdit.Messages;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;
using GlyphEdit.Model;

namespace GlyphEdit.ViewModels
{
    public class ColorPaletteStore
    {
        public HashSet<ColorPalette> ColorPalettes { get; private set; }

        public void DetectColorPalettes()
        {
            ColorPalettes = new HashSet<ColorPalette>();
            foreach (var file in Directory.GetFiles("Palettes", "*.png"))
            {
                var filename = Path.GetFullPath(file);
                try
                {
                    var pixelColors = BitmapUtils.LoadGlyphColors(filename);
                    var name = Path.GetFileNameWithoutExtension(file);
                    var glyphFont = ColorPalette.Create(name, pixelColors);
                    ColorPalettes.Add(glyphFont);
                }
                catch (Exception e)
                {
                    ColorPalettes.Add(ColorPalette.CreateInvalid(filename, "Could not load palette from file: " + e.Message));
                }
            }
            MessageBus.Publish(new ColorPaletteListLoadedEvent(ColorPalettes.ToArray()));
        }
    }
}
