using System.Windows.Media;

namespace GlyphEdit.Controls.BrushBar.PalettePicker
{
    /// <summary>
    /// Viewmodel of the ColorPaletteGrid context menu when mouse is pointing to an existing ColorPatch on the grid.
    /// </summary>
    public class ColorGridContextMenuViewModel
    {
        public Brush ColorPatchBrush { get; set; }
        public string ColorHexCode { get; set; }
    }
}
