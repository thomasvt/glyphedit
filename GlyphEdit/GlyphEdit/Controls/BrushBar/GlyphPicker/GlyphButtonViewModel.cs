using System;
using System.Windows;
using System.Windows.Media;
using GlyphEdit.ViewModels;

namespace GlyphEdit.Controls.BrushBar.GlyphPicker
{
    public class GlyphButtonViewModel
    : ViewModel
    {
        private bool _isPicked;

        public GlyphButtonViewModel(GlyphFontViewModel glyphFontViewModel, int glyphIndex, Int32Rect cropInFontBitmap)
        {
            GlyphFontViewModel = glyphFontViewModel;
            GlyphIndex = glyphIndex;
            CropInFontBitmap = cropInFontBitmap;
            var size = Math.Max(glyphFontViewModel.GlyphSize.X, glyphFontViewModel.GlyphSize.Y);
            Stretch = size > 12 ? Stretch.Uniform : Stretch.None;
        }

        public GlyphFontViewModel GlyphFontViewModel { get; }
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
