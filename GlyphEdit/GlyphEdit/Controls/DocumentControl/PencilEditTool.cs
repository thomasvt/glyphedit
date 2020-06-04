using System;
using GlyphEdit.Controls.DocumentView.Input;
using GlyphEdit.Models;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.DocumentView
{
    internal class PencilEditTool : EditTool
    {
        private readonly DocumentControl.DocumentControl _documentControl;
        private bool _isDrawing;
        private Point _previousDrawPosition;

        public PencilEditTool(DocumentControl.DocumentControl documentControl, WpfMouse mouse)
        : base(EditMode.Pencil)
        {
            _documentControl = documentControl;
            mouse.LeftButtonDown += MouseOnLeftButtonDown;
            mouse.LeftButtonUp += MouseOnLeftButtonUp;
            mouse.MouseMove += MouseOnMouseMove;
            _isDrawing = false;
        }

        private void MouseOnMouseMove(object sender, MouseMoveEventArgs e)
        {
            if (!_isDrawing)
                return;

            var documentCoords = _documentControl.GetDocumentCoordsAt(e.MouseState.Position);
            DrawLine(_previousDrawPosition, documentCoords);

            _previousDrawPosition = documentCoords;
        }

        private void DrawLine(Point from, Point to)
        {
            // from https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm ("All Cases" section)

            var dx = Math.Abs(to.X - from.X);
            var sx = from.X < to.X ? 1 : -1;
            var dy = -Math.Abs(to.Y - from.Y);
            var sy = from.Y < to.Y ? 1 : -1;
            var err = dx + dy;  /* error value e_xy */
            while (true)
            {
                DrawGlyph(new Point(from.X, from.Y));

                // Bresenham ct'd
                if (from.X == to.X && from.Y == to.Y) break;
                var e2 = err + err;
                if (e2 >= dy)
                {
                    err += dy; /* e_xy+e_x > 0 */
                    from.X += sx;
                }
                if (e2 <= dx) /* e_xy+e_y < 0 */
                {
                    err += dx;
                    from.Y += sy;
                }
            }
        }

        private void DrawGlyph(Point point)
        {
            if (!_documentControl.Document.IsInRange(point))
                return;
            ref var element = ref _documentControl.Document.GetElementRef(0, point);
            element.Glyph = _documentControl.CurrentGlyphIndex;
            element.Background = _documentControl.CurrentBackgroundColor;
            element.Foreground = _documentControl.CurrentForegroundColor;
        }

        private void MouseOnLeftButtonDown(object sender, MouseEventArgs e)
        {
            _previousDrawPosition = _documentControl.GetDocumentCoordsAt(e.MouseState.Position);
            DrawGlyph(_previousDrawPosition);
            _isDrawing = true;
        }

        private void MouseOnLeftButtonUp(object sender, MouseEventArgs e)
        {
            _isDrawing = false;
        }
    }
}
