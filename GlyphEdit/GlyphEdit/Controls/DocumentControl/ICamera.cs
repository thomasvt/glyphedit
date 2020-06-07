using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.DocumentControl
{
    public interface ICamera
    {
        Vector2 Position { get; }
        void SetPosition(Vector2 position);
        void ZoomToFitDocument(float duration);
    }
}