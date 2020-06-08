using System.Windows.Media;
using GlyphEdit.Model;
using GlyphEdit.ViewModels;

namespace GlyphEdit.Controls.BrushBar.PalettePicker
{
    public class ColorButtonViewModel
    : ViewModel
    {
        public ColorButtonViewModel(GlyphColor glyphColor)
        {
            GlyphColor = glyphColor;
            Color = glyphColor.ToWpfColor();
        }

        public Color Color { get; }
        public GlyphColor GlyphColor { get; }
    }
}
