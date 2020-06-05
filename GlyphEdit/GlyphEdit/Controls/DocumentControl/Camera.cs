using GlyphEdit.Controls.DocumentView.Input;
using GlyphEdit.Messages;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.DocumentView
{
    public class Camera : ICamera
    {
        private float _time;

        private readonly DocumentControl.DocumentControl _documentViewport;
        private Vector2 _panStartCameraPosition;
        private Point _panStartMousePosition;
        private Vector2 _viewportSize;

        private float _zoomAnimationDuration;
        private float _zoomFrom, _zoomTo, _zoomStartTime;
        private bool _isAnimatingZoom;

        public Camera(WpfMouse mouse, DocumentControl.DocumentControl documentViewport)
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
            MessageBus.Subscribe<GlyphChangedEvent>(e =>
            {
                if (e.PreviousGlyphFontViewModel != null && e.NewGlyphFontViewModel != null &&
                    e.PreviousGlyphFontViewModel.GlyphSize != e.NewGlyphFontViewModel.GlyphSize)
                {
                    var zoomChangeFactor = (float) e.NewGlyphFontViewModel.GlyphSize.Y / e.PreviousGlyphFontViewModel.GlyphSize.Y;
                    MoveTo(Position * zoomChangeFactor);
                    ZoomTo(Zoom / zoomChangeFactor);
                }
            });
        }

        public void Update(GameTime time)
        {
            _time = (float)time.TotalGameTime.TotalSeconds;

            if (_isAnimatingZoom)
            {
                var t = (_time - _zoomStartTime) / _zoomAnimationDuration;
                if (t >= 1f)
                {
                    ZoomTo(_zoomTo);
                    _isAnimatingZoom = false;
                }
                else
                {
                    ZoomTo((float) (_zoomFrom * (1 - t) + _zoomTo * t));
                }
            }
        }

        public void ZoomSmoothTo(float zoom, float duration)
        {
            _zoomFrom = Zoom;
            _zoomTo = zoom;
            _zoomStartTime = _time;
            _zoomAnimationDuration = duration;
            _isAnimatingZoom = true;
        }

        public void ZoomTo(float zoom)
        {
            Zoom = zoom;
            CalculateProjectionMatrix();
            MessageBus.Publish(new ZoomChangedEvent(Zoom));
        }

        private void ZoomIn()
        {
            var zoom = _isAnimatingZoom ? _zoomTo : Zoom;

            ZoomSmoothTo(zoom * 1.15f, 0.15f);
        }

        private void ZoomOut()
        {
            var zoom = _isAnimatingZoom ? _zoomTo : Zoom;
            ZoomSmoothTo(zoom / 1.15f, 0.15f);
        }

        public void MoveTo(Vector2 position)
        {
            Position = position;
            CalculateViewMatrices();
        }

        public void StartPan(Point position)
        {
            IsPanning = true;
            _panStartMousePosition = position;
            _panStartCameraPosition = Position;
        }

        public void UpdatePan(Point position)
        {
            MoveTo(_panStartCameraPosition + (_panStartMousePosition - position).ToVector2() / Zoom);
        }

        public void FinishPan()
        {
            IsPanning = false;
        }

        public void Reset()
        {
            ZoomTo(1f);
            MoveTo(new Vector2(_documentViewport.Document.Width * _documentViewport.CurrentGlyphMapTexture.GlyphWidth * 0.5f,
                _documentViewport.Document.Height * _documentViewport.CurrentGlyphMapTexture.GlyphHeight * 0.5f));
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

        public Vector2 Position { get; private set; }

        public bool IsPanning { get; private set; }
        public Matrix ViewMatrix { get; private set; }
        public Matrix ViewMatrixInverse { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }
        public Matrix ProjectionMatrixInverse { get; private set; }
        public float Zoom { get; private set; }
    }
}
