using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GlyphEdit.Model;

namespace GlyphEdit.ViewModels
{
    public static class BitmapUtils
    {
        /// <summary>
        /// Loads a font from file and replaces all Alpha by the average of R,G and B. Or: the amount of gray defines the amount of transparency, RGB is set to white.
        /// </summary>
        public static BitmapSource LoadAndCleanGlyphFontBitmap(string filename)
        {
            var bitmap = new BitmapImage(new Uri(filename, UriKind.Absolute));
            return ConvertPixels(bitmap, color =>
            {
                color.A = (byte) ((color.R + color.G + color.B) / 3);
                color.R = 0xFF;
                color.G = 0xFF;
                color.B = 0xFF;
                return color;
            });
        }

        /// <summary>
        /// Calls the converter for each pixel in the bitmap and returns a new BitmapSource with the converted pixels.
        /// </summary>
        public static BitmapSource ConvertPixels(BitmapSource bitmap, Func<Color, Color> converter)
        {
            var pixels = GetPixelData(bitmap);

            for (var i = 0; i < pixels.Length; i += 4)
            {
                ref var b = ref pixels[i];
                ref var g = ref pixels[i+1];
                ref var r = ref pixels[i+2];
                ref var a = ref pixels[i+3];

                var color = Color.FromArgb(a, r, g, b);
                var newColor = converter.Invoke(color);

                a = newColor.A;
                r = newColor.R;
                g = newColor.G;
                b = newColor.B;
            }

            var writableBitmap = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight, 96, 96, PixelFormats.Bgra32, null);
            writableBitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), pixels,
                bitmap.PixelWidth * 4, 0);
            return writableBitmap;
        }

        public static GlyphColor[,] LoadGlyphColors(string filename)
        {
            var bitmap = new BitmapImage(new Uri(filename, UriKind.Absolute));
            var pixelData = GetPixelData(bitmap);
            var pixels = new GlyphColor[bitmap.PixelWidth, bitmap.PixelHeight];
            var i = 0;
            for (var y = 0; y < bitmap.PixelHeight; y++)
            {
                for (var x = 0; x < bitmap.PixelWidth; x++)
                {
                    var b = pixelData[i];
                    var g = pixelData[i+1];
                    var r = pixelData[i+2];
                    var a = pixelData[i+3];
                    pixels[x, y] = new GlyphColor(r, g, b, a);
                    i += 4;
                }
            }

            return pixels;
        }

        private static byte[] GetPixelData(BitmapSource bitmapSource)
        {
            if (bitmapSource.Format == PixelFormats.Bgra32)
            {
                var stride = bitmapSource.PixelWidth * 4;
                var data = new byte[bitmapSource.PixelHeight * stride];
                bitmapSource.CopyPixels(data, stride, 0);
                return data;
            }

            if (bitmapSource.Format == PixelFormats.Bgr32)
            {
                var stride = bitmapSource.PixelWidth * 4;
                var data = new byte[bitmapSource.PixelHeight * stride];
                bitmapSource.CopyPixels(data, stride, 0);
                return data;
            }

            throw new NotSupportedException($"PixelFormat {bitmapSource.Format} is not supported. Only BGRA32 and BGR32.");
        }

        /// <summary>
        /// Replace a certain RGB into another RGB. Alphas are combined so that replacing with a semi transparent color combines the alpha with the alpha values in the bitmap.
        /// </summary>
        public static ImageSource ReplaceColor(BitmapSource sourceBitmap, Color oldColor, Color newColor)
        {
            return ConvertPixels(sourceBitmap, color =>
            {
                if (color.R == oldColor.R && color.G == oldColor.G && color.B == oldColor.B)
                {
                    return Color.FromArgb(
                        (byte)(newColor.A * color.A / 255), // mix both alphas
                        newColor.R, 
                        newColor.G, 
                        newColor.B);
                }
                return color;
            });
        }
    }
}
