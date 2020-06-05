using System;
using GlyphEdit.Controls.DocumentView.Input;
using GlyphEdit.Model;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.DocumentView
{
    internal class PencilEditTool : EditTool
    {
        private readonly DocumentControl.DocumentControl _documentControl;
        private bool _isDrawing;
        private VectorI _previousDrawPosition;

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

        private void DrawLine(VectorI from, VectorI to)
        {
            // from https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm ("All Cases" section)

            var x = from.X;
            var y = from.Y;
            var dx = Math.Abs(to.X - x);
            var sx = x < to.X ? 1 : -1;
            var dy = -Math.Abs(to.Y - y);
            var sy = y < to.Y ? 1 : -1;
            var err = dx + dy;  /* error value e_xy */
            while (true)
            {
                DrawGlyph(new VectorI(x, y));

                // Bresenham ct'd
                if (x == to.X && y == to.Y) break;
                var e2 = err + err;
                if (e2 >= dy)
                {
                    err += dy; /* e_xy+e_x > 0 */
                    x += sx;
                }
                if (e2 <= dx) /* e_xy+e_y < 0 */
                {
                    err += dx;
                    y += sy;
                }
            }
        }

        private void DrawGlyph(VectorI coords)
        {
            if (!_documentControl.Document.IsInRange(coords))
                return;

            ref var element = ref _documentControl.Document.GetElementRef(0, coords);
            if (_documentControl.Brush.IsGlyphEnabled) element.Glyph = _documentControl.Brush.GlyphIndex;
            if (_documentControl.Brush.IsForegroundEnabled) element.Foreground = _documentControl.Brush.ForegroundColor;
            if (_documentControl.Brush.IsBackgroundEnabled) element.Background = _documentControl.Brush.BackgroundColor;
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
