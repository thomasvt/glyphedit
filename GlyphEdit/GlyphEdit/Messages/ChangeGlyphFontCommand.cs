using GlyphEdit.Model;

namespace GlyphEdit.Messages
{
    public class ChangeGlyphFontCommand
    {
        public readonly GlyphFont GlyphFont;

        public ChangeGlyphFontCommand(GlyphFont glyphFont)
        {
            GlyphFont = glyphFont;
        }
    }
}
