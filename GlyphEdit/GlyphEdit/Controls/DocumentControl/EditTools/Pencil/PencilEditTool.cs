﻿using GlyphEdit.Controls.DocumentControl.Input;
using GlyphEdit.Model;
using GlyphEdit.Model.Manipulation;
using Microsoft.Xna.Framework.Input;

namespace GlyphEdit.Controls.DocumentControl.EditTools.Pencil
{
    internal class PencilEditTool : EditTool
    {
        private readonly DocumentControl _documentControl;
        private DocumentManipulationScope _manipulationScope;
        private LayerEditAccess _layerEditAccess;
        private VectorI _previousDrawPosition;
        private bool _hasDrawn;

        public PencilEditTool(DocumentControl documentControl)
        : base(EditMode.Pencil)
        {
            _documentControl = documentControl;
            _manipulationScope = null;
        }

        protected internal override void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            if (_manipulationScope == null)
                return;

            var documentCoords = _documentControl.GetDocumentCoordsAt(e.MouseState.Position);
            GridGeometry.ForEachCellInLine(_previousDrawPosition, documentCoords, DrawGlyph);

            _previousDrawPosition = documentCoords;
        }

        protected internal override void OnMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (_manipulationScope != null)
                return; // don't know if this ever happens, but it should just continue the current manipulation.

            _previousDrawPosition = _documentControl.GetDocumentCoordsAt(e.MouseState.Position);
            _manipulationScope = _documentControl.Document.Manipulator.BeginManipulation();
            _layerEditAccess = _manipulationScope.GetLayerEditAccess(_documentControl.ActiveLayerId);

            DrawGlyph(_previousDrawPosition);
        }

        protected internal override void OnKeyDown(object sender, KeyEventArgs args)
        {
            if (args.Key == Keys.Escape) 
                Cancel();
        }

        protected internal override void OnMouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (_manipulationScope == null)
                return;

            if (_hasDrawn)
                _manipulationScope.Commit();
            _manipulationScope = null;
            _layerEditAccess = null;
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

        private void Cancel()
        {
            if (_manipulationScope == null)
                return;

            _manipulationScope.Revert();
            _manipulationScope = null;
            _layerEditAccess = null;
        }
    }
}
