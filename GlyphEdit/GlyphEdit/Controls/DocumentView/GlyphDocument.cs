using System.Threading;

namespace GlyphEdit.Controls.DocumentView
{
    public class GlyphDocument
    {
        private Textel[,] _textels;

        public GlyphDocument(int width, int height)
        {
            _textels = new Textel[width, height];
        }
    }
}
