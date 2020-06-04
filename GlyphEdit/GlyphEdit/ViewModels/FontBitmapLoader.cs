using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GlyphEdit.ViewModels
{
    public static class FontBitmapLoader
    {
        // It's sad we have to program this ourselves in WPF while many less advanced frameworks offer simple API for this...

        public static BitmapSource Load(string filename)
        {
            var bitmap = new BitmapImage(new Uri(filename, UriKind.Absolute));
            //var writableBitmap = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight, 96, 96, PixelFormats.Bgra32, null);
            var pixels = GetPixels(bitmap);
            var writableBitmap = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight, 96, 96, PixelFormats.Bgra32, null);
            for (var i = 0; i < pixels.Length; i+=4)
            {
                var alpha = (byte)((pixels[i] + pixels[i+1] + pixels[i+2]) / 3);
                pixels[i] = 0xDD;
                pixels[i + 1] = 0xDD;
                pixels[i + 2] = 0xDD;
                pixels[i + 3] = alpha;
            }

            writableBitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), pixels, bitmap.PixelWidth * 4, 0);

            return writableBitmap;
        }

        private static byte[] GetPixels(BitmapSource bitmapSource)
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
    }
}
