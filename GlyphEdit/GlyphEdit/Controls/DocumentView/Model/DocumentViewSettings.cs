namespace GlyphEdit.Controls.DocumentView.Model
{
    public class DocumentViewSettings
    {
        public int GlyphSize { get; set; }
        public static DocumentViewSettings Default => new DocumentViewSettings
        {
            GlyphSize = 12
        };
    }
}
