using GlyphEdit.Controls.DocumentView.Input;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.DocumentView
{
    public class Camera : ICamera
    {
        private Vector2 _panStartCameraPosition;
        private Point _panStartMousePosition;

        public Camera(GlyphMouse mouse)
        {
            mouse.MiddleButtonDown += (sender, args) => StartPan(args.MouseState.Position);
            mouse.MiddleButtonUp += (sender, args) => FinishPan();
            mouse.Moved += (sender, args) =>
            {
                if (IsPanning) UpdatePan(args.MouseState.Position);
            };
        }

        public void StartPan(Point position)
        {
            IsPanning = true;
            _panStartMousePosition = position;
            _panStartCameraPosition = Position;
        }

        public void MoveTo(Vector2 position)
        {
            Position = position;
        }

        public void FinishPan()
        {
            IsPanning = false;
        }

        public void UpdatePan(Point position)
        {
            Position = _panStartCameraPosition + (_panStartMousePosition - position).ToVector2();
        }

        public Vector2 Position { get; private set; }
        public bool IsPanning { get; private set; }
    }
}
