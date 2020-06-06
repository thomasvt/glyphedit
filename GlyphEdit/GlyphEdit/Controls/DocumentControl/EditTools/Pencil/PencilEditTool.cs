using GlyphEdit.Controls.DocumentControl.Input;
using GlyphEdit.Model;
using GlyphEdit.Model.Manipulation;
using Microsoft.Xna.Framework.Input;
using Keyboard = GlyphEdit.Controls.DocumentControl.Input.Keyboard;
using Mouse = GlyphEdit.Controls.DocumentControl.Input.Mouse;

namespace GlyphEdit.Controls.DocumentControl.EditTools.Pencil
{
    internal class PencilEditTool : EditTool
    {
        private readonly DocumentControl _documentControl;
        private DocumentManipulationScope _manipulationScope;
        private LayerEditAccess _layerEditAccess;
        private VectorI _previousDrawPosition;
        private bool _hasDrawn;

        public PencilEditTool(DocumentControl documentControl, Mouse mouse, Keyboard keyboard)
        : base(EditMode.Pencil)
        {
            _documentControl = documentControl;
            mouse.LeftButtonDown += MouseOnLeftButtonDown;
            mouse.LeftButtonUp += MouseOnLeftButtonUp;
            mouse.MouseMove += MouseOnMouseMove;
            keyboard.KeyDown += (sender, args) => { if (args.Key == Keys.Escape) Cancel(); };
            _manipulationScope = null;
        }

        private void MouseOnMouseMove(object sender, MouseMoveEventArgs e)
        {
            if (_manipulationScope == null)
                return;

            var documentCoords = _documentControl.GetDocumentCoordsAt(e.MouseState.Position);
            GridGeometry.ForEachCellInLine(_previousDrawPosition, documentCoords, DrawGlyph);

            _previousDrawPosition = documentCoords;
        }

        private void DrawGlyph(VectorI coords)
        {
            if (!_documentControl.Document.IsInRange(coords))
                return;

            _hasDrawn = true;
            ref var element = ref _layerEditAccess.GetElementRef(coords);
            if (_documentControl.Brush.IsGlyphEnabled) element.Glyph = _documentControl.Brush.GlyphIndex;
            if (_documentControl.Brush.IsForegroundEnabled) element.Foreground = _documentControl.Brush.ForegroundColor;
            if (_documentControl.Brush.IsBackgroundEnabled) element.Background = _documentControl.Brush.BackgroundColor;
        }

        private void MouseOnLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (_manipulationScope != null)
                return; // don't know if this ever happens, but it should just continue the current manipulation.

            _previousDrawPosition = _documentControl.GetDocumentCoordsAt(e.MouseState.Position);
            _manipulationScope = _documentControl.Document.Manipulator.BeginManipulation();
            _layerEditAccess = _manipulationScope.GetLayerEditAccess(_documentControl.ActiveLayerId);

            DrawGlyph(_previousDrawPosition);
        }

        private void Cancel()
        {
            if (_manipulationScope == null)
                return;

            _manipulationScope.Revert();
            _manipulationScope = null;
            _layerEditAccess = null;
        }

        private void MouseOnLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (_manipulationScope == null)
                return;

            if (_hasDrawn)
                _manipulationScope.Commit();
            _manipulationScope = null;
            _layerEditAccess = null;
        }
    }
}
