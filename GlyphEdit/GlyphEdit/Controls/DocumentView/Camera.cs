using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.DocumentView
{
    public class Camera
    {
        private bool _isPanning;
        private Vector2 _panStartCameraPosition;
        private Point _panStartMousePosition;

        public void StartPan(Point position)
        {
            _isPanning = true;
            _panStartMousePosition = position;
            _panStartCameraPosition = Position;
        }

        public Vector2 Position { get; set; }

        public void EndPan()
        {
            _isPanning = false;
        }

        public bool IsPanning() => _isPanning;

        public void UpdatePan(Point position)
        {
            Position = _panStartCameraPosition + (_panStartMousePosition - position).ToVector2();
        }
    }
}
