using System;
using System.Data.SqlTypes;
using GlyphEdit.Controls.DocumentView.Input;
using GlyphEdit.Controls.DocumentView.Model;

namespace GlyphEdit.Controls.DocumentView
{
    internal class PencilEditTool : EditTool
    {
        private readonly DocumentViewport _documentViewport;
        private bool _isDrawing;

        public PencilEditTool(DocumentViewport documentViewport, GlyphMouse mouse)
        : base(EditMode.Pencil)
        {
            _documentViewport = documentViewport;
            mouse.LeftButtonDown += MouseOnLeftButtonDown;
            mouse.LeftButtonUp += MouseOnLeftButtonUp;
            mouse.Moved += MouseOnMoved;
            _isDrawing = false;
        }

        private void MouseOnMoved(object sender, MouseMoveEventArgs e)
        {
            if (!_isDrawing)
                return;

            var documentCoords = _documentViewport.GetDocumentCoordsAt(e.MouseState.Position);
            if (!_documentViewport.Document.IsInRange(documentCoords))
                return;
            ref var element = ref _documentViewport.Document.GetElementRef(0, documentCoords);
            element.Glyph = 1;
            element.Background = new GlyphColor(200, 0, 0, 255);
        }

        private void MouseOnLeftButtonDown(object sender, MouseEventArgs e)
        {
            _isDrawing = true;
        }

        private void MouseOnLeftButtonUp(object sender, MouseEventArgs e)
        {
            _isDrawing = false;
        }
    }
}
