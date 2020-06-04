using System;
using System.Windows;
using System.Windows.Media;
using GlyphEdit.Models;
using GlyphEdit.ViewModels;

namespace GlyphEdit.Controls.PanelsBar.GlyphPicker
{
    public class GlyphButtonViewModel
    : ViewModel
    {
        private bool _isPicked;

        public GlyphButtonViewModel(GlyphFont glyphFont, int glyphIndex, Int32Rect cropInFontBitmap)
        {
            GlyphFont = glyphFont;
            GlyphIndex = glyphIndex;
            CropInFontBitmap = cropInFontBitmap;
            var size = Math.Max(glyphFont.GlyphSize.X, glyphFont.GlyphSize.Y);
            Stretch = size > 12 ? Stretch.Uniform : Stretch.None;
        }

        public GlyphFont GlyphFont { get; }
        public int GlyphIndex { get; }
        public Int32Rect CropInFontBitmap { get; }

        public bool IsPicked
        {
            get => _isPicked;
            set
            {
                if (value == _isPicked) return;
                _isPicked = value;
                OnPropertyChanged();
            }
        }

        public Stretch Stretch { get; set; }
    }
}
