using GlyphEdit.Controls.DocumentView.Input;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.DocumentView
{
    public class Camera : ICamera
    {
        private readonly DocumentControl _documentViewport;
        private Vector2 _panStartCameraPosition;
        private Point _panStartMousePosition;
        private Vector2 _position;
        private Vector2 _viewportSize;

        public Camera(WpfMouse mouse, DocumentControl documentViewport)
        {
            _documentViewport = documentViewport;
            Zoom = 1f;
            mouse.MiddleButtonDown += (sender, args) => StartPan(args.MouseState.Position);
            mouse.MiddleButtonUp += (sender, args) => FinishPan();
            mouse.MouseMove += (sender, args) =>
            {
                if (IsPanning) UpdatePan(args.MouseState.Position);
            };
            mouse.MouseWheel += (sender, args) =>
            {
                if (args.WheelValueChange > 0)
                {
                    ZoomIn();
                }
                else
                {
                    ZoomOut();
                }
            };
        }

        private void ZoomIn()
        {
            Zoom *= 1.1f;
            CalculateProjectionMatrix();
        }

        private void ZoomOut()
        {
            Zoom /= 1.1f;
            CalculateProjectionMatrix();
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

        public void Reset()
        {
            Zoom = 1f;
            MoveTo(new Vector2(_documentViewport.Document.Width * _documentViewport.CurrentGlyphMapTexture.GlyphWidth * 0.5f,
                _documentViewport.Document.Height * _documentViewport.CurrentGlyphMapTexture.GlyphHeight * 0.5f));
        }

        public void UpdatePan(Point position)
        {
            Position = _panStartCameraPosition + (_panStartMousePosition - position).ToVector2() / Zoom;
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

        public Point GetDocumentPosition(Point screenPosition)
        {
            var halfScreenX = _viewportSize.X * 0.5f;
            var halfScreenY = _viewportSize.Y * 0.5f;
            var x = (screenPosition.X - halfScreenX) / Zoom + Position.X;
            var y = (screenPosition.Y - halfScreenY) / Zoom + Position.Y;

            return new Point((int)x, (int)y);
        }

        public void SetViewport(Vector2 viewportSize)
        {
            _viewportSize = viewportSize;
            CalculateProjectionMatrix();
        }

        private void CalculateProjectionMatrix()
        {
            ProjectionMatrix = Matrix.CreateOrthographic(_viewportSize.X / Zoom, _viewportSize.Y / Zoom, 0.1f, 10f);
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
        public float Zoom { get; private set; }
    }
}
