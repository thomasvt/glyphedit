using System;
using GlyphEdit.Controls.DocumentControl.Input;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.DocumentControl
{
    public class Camera : ICamera
    {
        private float _time;

        private readonly DocumentControl _documentControl;
        private Vector2 _panStartCameraPosition;
        private Point _panStartMousePosition;
        private Vector2 _viewportSize;

        private float _transitionDuration, _transitionStartTime;
        private float _zoomFrom, _zoomTo;
        private Vector2 _zoomTowardsPosition;
        private bool _isAnimatingZoom;

        public Camera(Mouse mouse, DocumentControl documentControl)
        {
            _documentControl = documentControl;
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
                    ZoomIn(GetDocumentPosition(args.MouseState.Position).ToVector2());
                }
                else
                {
                    ZoomOut(GetDocumentPosition(args.MouseState.Position).ToVector2());
                }
            };
            MessageBus.Subscribe<GlyphChangedEvent>(e =>
            {
                if (e.PreviousGlyphFontViewModel != null && e.NewGlyphFontViewModel != null &&
                    e.PreviousGlyphFontViewModel.GlyphSize != e.NewGlyphFontViewModel.GlyphSize)
                {
                    var zoomChangeFactor = (float)e.NewGlyphFontViewModel.GlyphSize.Y / e.PreviousGlyphFontViewModel.GlyphSize.Y;
                    SetPosition(Position * zoomChangeFactor);
                    SetZoom(Zoom / zoomChangeFactor);
                }
            });
        }

        public void Update(GameTime time)
        {
            _time = (float)time.TotalGameTime.TotalSeconds;

            if (_isAnimatingZoom)
            {
                var t = (_time - _transitionStartTime) / _transitionDuration;

                var previousZoom = Zoom;
                float newZoom;
                if (t >= 1f)
                {
                    // end of animation reached
                    newZoom = _zoomTo;
                    _isAnimatingZoom = false;
                }
                else
                {
                    newZoom = _zoomFrom * (1 - t) + _zoomTo * t;
                }
                SetZoom(newZoom);
                // keep zoomTowardsPosition at the same screencoords by moving the camera proportionately
                SetPosition(Position + (newZoom / previousZoom - 1f) * (_zoomTowardsPosition - Position));
            }
        }

        /// <param name="focusPosition">"Zoom towards" location in documentcoords</param>
        public void ZoomSmoothTo(float zoom, float duration, Vector2? focusPosition = null)
        {
            _zoomFrom = Zoom;
            _zoomTo = zoom;

            _transitionStartTime = _time;
            _transitionDuration = duration;
            _isAnimatingZoom = true;

            _zoomTowardsPosition = focusPosition ?? Position; // for "zoom towards mouse" behavior
        }

        public void ZoomToFitDocument(float duration)
        {
            var documentPixelSize =
                new Vector2(_documentControl.Document.Width * _documentControl.CurrentGlyphMapTexture.GlyphWidth,
                    _documentControl.Document.Height * _documentControl.CurrentGlyphMapTexture.GlyphHeight);
            var zoomX = _viewportSize.X * 0.9f / documentPixelSize.X;
            var zoomY = _viewportSize.Y * 0.9f / documentPixelSize.Y;
            SetPosition(documentPixelSize * 0.5f);
            ZoomSmoothTo(MathHelper.Min(zoomX, zoomY), duration);
        }

        public void SetZoom(float zoom)
        {
            Zoom = zoom;
            CalculateProjectionMatrix();
            MessageBus.Publish(new ZoomChangedEvent(Zoom));
        }

        private void ZoomIn(Vector2? focusPosition = null)
        {
            var zoom = _isAnimatingZoom ? _zoomTo : Zoom;

            ZoomSmoothTo(zoom * 1.15f, 0.15f, focusPosition);
        }

        private void ZoomOut(Vector2? focusPosition = null)
        {
            var zoom = _isAnimatingZoom ? _zoomTo : Zoom;
            ZoomSmoothTo(zoom / 1.15f, 0.15f, focusPosition);
        }

        public void SetPosition(Vector2 position)
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
            SetPosition(_panStartCameraPosition + (_panStartMousePosition - position).ToVector2() / Zoom);
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

        /// <summary>
        /// Converst standard pixel coords (with topleft = 0,0) to pixel coords around with the center of the view = 0,0 instead.
        /// </summary>
        public Point GetScreenPositionFromCenter(Point screenPosition)
        {
            var halfScreenX = _viewportSize.X * 0.5f;
            var halfScreenY = _viewportSize.Y * 0.5f;
            var x = screenPosition.X - halfScreenX;
            var y = screenPosition.Y - halfScreenY;

            return new Point((int)x, (int)y);
        }

        public Point GetDocumentPosition(Point screenPosition)
        {
            var (x, y) = GetScreenPositionFromCenter(screenPosition);
            return new Point((int)(x / Zoom + Position.X), (int)(y / Zoom + Position.Y));
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
