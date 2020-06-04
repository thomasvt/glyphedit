using System.IO;
using System.IO.Compression;
using GlyphEdit.Models;

namespace GlyphEdit.Persistence
{
    public static class DocumentSaver
    {
        public static void Save(Document document, string filename)
        {
            if (File.Exists(filename))
                File.Delete(filename); // openwrite writes from beginning, but does not remove data that was already in the file, so any data further than we write remains untouched.

            const int documentVersion = 1;
            using (var stream = File.OpenWrite(filename))
            {
                var zipStream = new GZipStream(stream, CompressionLevel.Optimal);
                using (var writer = new BinaryWriter(zipStream))
                {
                    writer.Write(documentVersion);
                    writer.Write(document.Width);
                    writer.Write(document.Height);
                    writer.Write(document.LayerCount);
                    foreach (var layer in document.Layers)
                    {
                        foreach (var element in layer.Elements)
                        {
                            writer.Write((char)element.Glyph);
                            writer.Write(element.Background.ToPacked());
                            writer.Write(element.Foreground.ToPacked());
                        }
                    }
                }
            }
        }
    }
}
