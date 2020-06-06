using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace GlyphEdit.Model.Persistence
{
    public static class DocumentLoader
    {
        public static Document Load(string filename)
        {
            using (var stream = File.OpenRead(filename))
            {
                var zipStream = new GZipStream(stream, CompressionMode.Decompress);
                using (var reader = new BinaryReader(zipStream))
                {
                    var documentVersion = reader.ReadInt32();
                    var width = reader.ReadInt16();
                    var height = reader.ReadInt16();
                    var layerCount = reader.ReadInt16();
                    
                    var layers = new List<Layer>(layerCount);
                    for (var i = 0; i < layerCount; i++)
                    {
                        var layer = new Layer(Guid.NewGuid(), width, height);
                        for (var x = 0; x < width; x++)
                        { 
                            for (var y = 0; y < height; y++)
                            {
                                var glyph = reader.ReadChar();
                                var background = GlyphColor.FromPacked(reader.ReadUInt32());
                                var foreground = GlyphColor.FromPacked(reader.ReadUInt32());
                                layer.Elements[x, y] = new DocumentElement
                                {
                                    Glyph = glyph,
                                    Foreground = foreground,
                                    Background = background
                                };
                            }
                        }
                        layers.Add(layer);
                    }

                    var document = new Document(width, height, true, layers);

                    return document;
                }
            }
        }
    }
}
