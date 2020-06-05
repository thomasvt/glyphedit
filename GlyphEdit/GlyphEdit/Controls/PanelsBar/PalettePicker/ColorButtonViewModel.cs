using System.Windows.Media;
using GlyphEdit.Model;
using GlyphEdit.ViewModels;

namespace GlyphEdit.Controls.PanelsBar.PalettePicker
{
    public class ColorButtonViewModel
    : ViewModel
    {
        public ColorButtonViewModel(GlyphColor glyphColor)
        {
            GlyphGlyphColor = glyphColor;
            Color = glyphColor.ToWpfColor();
        }

        public Color Color { get; }
        public GlyphColor GlyphGlyphColor { get; }
    }
}
