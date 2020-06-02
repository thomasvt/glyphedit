using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.DocumentView.Model
{
    public class DocumentViewSettings
    {
        public static DocumentViewSettings Default => new DocumentViewSettings
        {
            GlyphFont = new GlyphFont
            {
                FontName = "cp437_20x20.png",
                GlyphSize = new Point(20, 20)
            }
        };

        public GlyphFont GlyphFont { get; set; }
    }
}
