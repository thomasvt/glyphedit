using GlyphEdit.Model;

namespace GlyphEdit.Controls.DocumentControl.EditTools
{
    public class GlyphBrush
    {
        public GlyphColor ForegroundColor { get; internal set; }
        public GlyphColor BackgroundColor { get; internal set; }
        public int GlyphIndex { get; internal set; }
        public bool IsGlyphEnabled { get; internal set; }
        public bool IsForegroundEnabled { get; internal set; }
        public bool IsBackgroundEnabled { get; internal set; }
    }
}
