using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GlyphEdit.Wpf.ColorGrid
{
    public class ColorPaletteGrid : Control
    {
        // note on drag drop: 
        // Doing this full WPF style (ItemsControl, Databinding with ItemContainer checking and styling, drag drop + visual feedback which WPF doesn't do out of the box) is a lot of work.
        // So, we implement the basics from scratch, which is a lot easier code.

        private ColorPatch _grabbedPatch;
        private Point _dragLocation;
        private bool _isDragging; // used to see difference between clicking and dragging
        private Point _mouseDownLocation;

        static ColorPaletteGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPaletteGrid), new FrameworkPropertyMetadata(typeof(ColorPaletteGrid)));
        }

        public ColorPatch GetColorPatchAt(Point positionInControl)
        {
            return ColorPatches.SingleOrDefault(p =>
                p.GridLocation.X * _patchSize <= positionInControl.X && p.GridLocation.Y * _patchSize <= positionInControl.Y &&
                (p.GridLocation.X + 1) * _patchSize > positionInControl.X && (p.GridLocation.Y + 1) * _patchSize > positionInControl.Y);
        }

        public PointI GetGridLocationAt(Point location)
        {
            return new PointI((int)(location.X / _patchSize), (int)(location.Y / _patchSize));
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (ColorPatches == null || ColorPatches.Count == 0)
                return Size.Empty;

            // calculate desired size:
            var patchSize = constraint.Width / ColumnCount;
            return new Size(constraint.Width, RowCount * patchSize);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            _patchSize = arrangeBounds.Width / ColumnCount;
            return base.ArrangeOverride(arrangeBounds);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Background, null, new Rect(0, 0, ActualWidth, ActualHeight));

            if (ColorPatches == null || ColorPatches.Count == 0)
                return;

            foreach (var colorPatch in ColorPatches)
            {
                if (colorPatch == _grabbedPatch && _isDragging)
                    continue;
                
                drawingContext.DrawRectangle(new SolidColorBrush(colorPatch.Color), new Pen(ColorPatchBorderBrush, 1f), new Rect(_patchSize * colorPatch.GridLocation.X, _patchSize * colorPatch.GridLocation.Y, _patchSize, _patchSize));
            }

            if (_grabbedPatch != null && _isDragging)
            {
                var halfPatchSize = _patchSize * 0.5;
                drawingContext.DrawRectangle(new SolidColorBrush(_grabbedPatch.Color), new Pen(new SolidColorBrush(Color.FromArgb(255, 200, 200, 200)), 1), new Rect(_dragLocation.X - halfPatchSize, _dragLocation.Y - halfPatchSize, _patchSize, _patchSize));
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.ChangedButton != MouseButton.Left)
                return;

            _dragLocation = e.GetPosition(this);
            _mouseDownLocation = _dragLocation;
            var patch = GetColorPatchAt(_dragLocation);
            if (patch != null)
            {
                _isDragging = false; // we are not yet dragging until mouse has moved a small distance with the grabbed patch
                _grabbedPatch = patch;
                InvalidateVisual();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_grabbedPatch != null)
            {
                _dragLocation = e.GetPosition(this);
                if ((_dragLocation - _mouseDownLocation).LengthSquared > 9)
                    _isDragging = true; // only start actual dragging if mouse moved >3 px, else it's a "click"

                if (_dragLocation.X < 0 || _dragLocation.X >= ActualWidth || _dragLocation.Y < 0 ||
                    _dragLocation.Y >= ActualHeight)
                {
                    CancelDrag();
                }
                else
                {
                    InvalidateVisual();
                }
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.ChangedButton == MouseButton.Right)
            {
                var patch = GetColorPatchAt(e.GetPosition(this));
                if (patch != null)
                    RaiseEvent(new ColorPatchRoutedEventArgs(ColorPatchRightClickEvent, patch));
                return;
            }

            if (_grabbedPatch != null)
            {
                if (_isDragging)
                {
                    DropPatch(e.GetPosition(this));
                }
                else
                {
                    // it's a click:
                    if (e.ChangedButton == MouseButton.Left)
                    {
                        var clickedPatch = _grabbedPatch;
                        CancelDrag();
                        RaiseEvent(new ColorPatchRoutedEventArgs(ColorPatchLeftClickEvent, clickedPatch));
                    }
                }
            }
        }

        private void CancelDrag()
        {
            if (_grabbedPatch == null)
                return;

            _isDragging = false;
            _grabbedPatch = null;
            InvalidateVisual();
        }

        private void DropPatch(Point location)
        {
            if (GetColorPatchAt(location) == null)
            {
                _grabbedPatch.GridLocation = new PointI((int)(location.X / _patchSize), (int)(location.Y / _patchSize));
                RaiseColorsModifiedEvent();
            }
            _grabbedPatch = null;
            InvalidateVisual();
        }

        private void RaiseColorsModifiedEvent()
        {
            RaiseEvent(new RoutedEventArgs(ColorsModifiedEvent));
        }


        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            CancelDrag();
        }


        #region Dependency properties

        public static readonly DependencyProperty ColorPatchesProperty = DependencyProperty.Register(
            "ColorPatches", typeof(List<ColorPatch>), typeof(ColorPaletteGrid), new FrameworkPropertyMetadata(default(List<ColorPatch>), FrameworkPropertyMetadataOptions.AffectsMeasure)
            {
                PropertyChangedCallback = (o, args) =>
                {
                    if (o is ColorPaletteGrid colorGrid)
                    {
                        var neededRowCount = colorGrid.ColorPatches.Max(p => p.GridLocation.Y) + 1;
                        if (neededRowCount > colorGrid.RowCount)
                            colorGrid.RowCount = neededRowCount;
                    }
                }
            });

        public List<ColorPatch> ColorPatches
        {
            get => (List<ColorPatch>)GetValue(ColorPatchesProperty);
            set => SetValue(ColorPatchesProperty, value);
        }

        

        public static readonly DependencyProperty ColorPatchBorderBrushProperty = DependencyProperty.Register(
            "ColorPatchBorderBrush", typeof(Brush), typeof(ColorPaletteGrid), new PropertyMetadata(default(Brush)));

        public Brush ColorPatchBorderBrush
        {
            get => (Brush)GetValue(ColorPatchBorderBrushProperty);
            set => SetValue(ColorPatchBorderBrushProperty, value);
        }



        public static readonly DependencyProperty ColumnCountProperty = DependencyProperty.Register(
            "ColumnCount", typeof(int), typeof(ColorPaletteGrid), new FrameworkPropertyMetadata(8, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public int ColumnCount
        {
            get => (int)GetValue(ColumnCountProperty);
            set => SetValue(ColumnCountProperty, value);
        }



        public static readonly DependencyProperty RowCountProperty = DependencyProperty.Register(
            "RowCount", typeof(int), typeof(ColorPaletteGrid), new FrameworkPropertyMetadata(4, FrameworkPropertyMetadataOptions.AffectsMeasure));

        private double _patchSize;

        public int RowCount
        {
            get => (int)GetValue(RowCountProperty);
            set => SetValue(RowCountProperty, value);
        }



        #endregion

        #region Routed Events

        public delegate void ColorPatchRoutedEventHandler(object sender, ColorPatchRoutedEventArgs e);

        public static readonly RoutedEvent ColorPatchLeftClickEvent = EventManager.RegisterRoutedEvent(
            "ColorPatchLeftClick", RoutingStrategy.Bubble, typeof(ColorPatchRoutedEventHandler), typeof(ColorPaletteGrid));

        public event ColorPatchRoutedEventHandler ColorPatchLeftClick
        {
            add => AddHandler(ColorPatchLeftClickEvent, value);
            remove => RemoveHandler(ColorPatchLeftClickEvent, value);
        }

        public static readonly RoutedEvent ColorPatchRightClickEvent = EventManager.RegisterRoutedEvent(
            "ColorPatchRightClick", RoutingStrategy.Bubble, typeof(ColorPatchRoutedEventHandler), typeof(ColorPaletteGrid));

        public event ColorPatchRoutedEventHandler ColorPatchRightClick
        {
            add => AddHandler(ColorPatchRightClickEvent, value);
            remove => RemoveHandler(ColorPatchRightClickEvent, value);
        }

        public static readonly RoutedEvent ColorsModifiedEvent = EventManager.RegisterRoutedEvent(
            "ColorsModified", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ColorPaletteGrid));

        public event RoutedEventHandler ColorsModified
        {
            add => AddHandler(ColorsModifiedEvent, value);
            remove => RemoveHandler(ColorsModifiedEvent, value);
        }

        #endregion
    }
}
