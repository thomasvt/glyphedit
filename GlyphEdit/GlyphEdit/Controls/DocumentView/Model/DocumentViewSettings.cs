using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.DocumentView.Model
{
    public class DocumentViewSettings
    {
        public float Zoom { get; set; }
        public static DocumentViewSettings Default => new DocumentViewSettings
        {
            Zoom = 1f,
            GlyphFont = new GlyphFont
            {
                FontName = "cp437_20x20.png",
                GlyphSize = new Point(20, 20)
            }
        };

        public GlyphFont GlyphFont { get; set; }
    }
}
