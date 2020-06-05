using System.Windows;

namespace GlyphEdit.ViewModels
{
    /// <summary>
    /// GlyphFont extensions for WPF
    /// </summary>
    public static class GlyphFontExtensions
    {
        public static Int32Rect GetGlyphCropRectangle(this GlyphFontViewModel glyphFontViewModel, int glyphIndex)
        {
            var x = glyphIndex % glyphFontViewModel.ColumnCount;
            var y = glyphIndex / glyphFontViewModel.ColumnCount;
            return new Int32Rect(x * glyphFontViewModel.GlyphSize.X, y * glyphFontViewModel.GlyphSize.Y, glyphFontViewModel.GlyphSize.X, glyphFontViewModel.GlyphSize.Y);
        }
    }
}
