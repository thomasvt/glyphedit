using GlyphEdit.Controls.DocumentControl.Input;
using GlyphEdit.Model;
using GlyphEdit.Model.Manipulation;
using Microsoft.Xna.Framework.Input;
using Keyboard = GlyphEdit.Controls.DocumentControl.Input.Keyboard;
using Mouse = GlyphEdit.Controls.DocumentControl.Input.Mouse;

namespace GlyphEdit.Controls.DocumentControl.EditTools.Eraser
{
    internal class EraserEditTool : EditTool
    {
        private readonly DocumentControl _documentControl;
        private DocumentManipulationScope _manipulationScope;
        private LayerEditAccess _layerEditAccess;
        private VectorI _previousDrawPosition;
        private bool _hasDrawn;

        public EraserEditTool(DocumentControl documentControl, Mouse mouse, Keyboard keyboard)
        : base(EditMode.Eraser)
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
            GridGeometry.ForEachCellInLine(_previousDrawPosition, documentCoords, EraseGlyph);

            _previousDrawPosition = documentCoords;
        }

        private void EraseGlyph(VectorI coords)
        {
            if (!_documentControl.Document.IsInRange(coords))
                return;

            _hasDrawn = true;
            ref var element = ref _layerEditAccess.GetElementRef(coords);
            if (_documentControl.Brush.IsGlyphEnabled) element.Glyph = 0;
            if (_documentControl.Brush.IsForegroundEnabled) element.Foreground = new GlyphColor(0, 0, 0, 0);
            if (_documentControl.Brush.IsBackgroundEnabled) element.Background = new GlyphColor(0, 0, 0, 0);
        }

        private void MouseOnLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (_manipulationScope != null)
                return; // don't know if this ever happens, but it should just continue the current manipulation.

            _previousDrawPosition = _documentControl.GetDocumentCoordsAt(e.MouseState.Position);
            _manipulationScope = _documentControl.Document.Manipulator.BeginManipulation();
            _layerEditAccess = _manipulationScope.GetLayerEditAccess(_documentControl.ActiveLayerId);

            EraseGlyph(_previousDrawPosition);
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
