using System;
using GlyphEdit.Controls.DocumentView.Input;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.DocumentView
{
    public class Camera : ICamera
    {
        private Vector2 _panStartCameraPosition;
        private Point _panStartMousePosition;
        private Vector2 _position;
        private Vector2 _viewportSize;

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

        private void CalculateViewMatrices()
        {
            ViewMatrix = Matrix.CreateLookAt(new Vector3(Position, -5f), new Vector3(Position, 0f), Vector3.Down);
            ViewMatrixInverse = Matrix.Invert(ViewMatrix);
        }

        public void UpdatePan(Point position)
        {
            Position = _panStartCameraPosition + (_panStartMousePosition - position).ToVector2();
        }

        public Point GetDocumentPosition(Point screenPosition)
        {
            var halfScreenX = _viewportSize.X * 0.5f;
            var halfScreenY = _viewportSize.Y * 0.5f;
            var x = screenPosition.X - halfScreenX + Position.X;
            var y = screenPosition.Y - halfScreenY + Position.Y;

            return new Point((int)x, (int)y);
        }

        public void SetViewport(Vector2 viewportSizePx)
        {
            _viewportSize = viewportSizePx;
            ProjectionMatrix = Matrix.CreateOrthographic(viewportSizePx.X, viewportSizePx.Y, 0.1f, 10f);
            ProjectionMatrixInverse = Matrix.Invert(ProjectionMatrix);
        }

        public Vector2 Position
        {
            get => _position;
            private set
            {
                _position = value;
                CalculateViewMatrices();
            }
        }

        public bool IsPanning { get; private set; }
        public Matrix ViewMatrix { get; private set; }
        public Matrix ViewMatrixInverse { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }
        public Matrix ProjectionMatrixInverse { get; private set; }
    }
}
