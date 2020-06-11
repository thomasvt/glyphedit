using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using GlyphEdit.Controls.DocumentControl.EditTools;
using GlyphEdit.Controls.DocumentControl.EditTools.Eraser;
using GlyphEdit.Controls.DocumentControl.EditTools.Pencil;
using GlyphEdit.Controls.DocumentControl.Rendering;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;
using GlyphEdit.Model;
using GlyphEdit.ViewModels;
using GlyphEdit.Wpf;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using Keyboard = GlyphEdit.Controls.DocumentControl.Input.Keyboard;
using Mouse = GlyphEdit.Controls.DocumentControl.Input.Mouse;
using Point = Microsoft.Xna.Framework.Point;

namespace GlyphEdit.Controls.DocumentControl
{
    public class DocumentControl : WpfGame
    {
        private EditTool _currentEditTool;
        private IGraphicsDeviceService _graphicsDeviceManager;
        private Mouse _mouse;
        private Keyboard _keyboard;
        private DocumentRenderer _documentRenderer;
        private Renderer _renderer;
        private Camera _camera;
        private readonly Dictionary<GlyphFontViewModel, GlyphMapTexture> _glyphMapTextures;
        
        public DocumentControl()
        {
            _glyphMapTextures = new Dictionary<GlyphFontViewModel, GlyphMapTexture>();
            Brush = new GlyphBrush();

            // this is a control, only subscribe to Events. All Commands must go through a ViewModel.
            MessageBus.Subscribe<DocumentOpenedEvent>(e => { OpenDocument(e.Document.Document); });
            MessageBus.Subscribe<ZoomChangeRequestedEvent>(e => { _camera.ZoomSmoothTo(e.Zoom, 0.15f); });
            MessageBus.Subscribe<ZoomToFitRequestedEvent>(e => { _camera.ZoomToFitDocument(0.15f); });
            MessageBus.Subscribe<EditModeChangedEvent>(e => ChangeEditMode(e.EditMode));
            MessageBus.Subscribe<GlyphChangedEvent>(e => ChangeGlyph(e.NewGlyphFontViewModel, e.NewGlyphIndex));
            MessageBus.Subscribe<ForegroundColorChangedEvent>(w => ChangeForegroundColor(w.Color));
            MessageBus.Subscribe<BackgroundColorChangedEvent>(w => ChangeBackgroundColor(w.Color));
            MessageBus.Subscribe<BrushGlyphEnabledChangedEvent>(e => Brush.IsGlyphEnabled = e.IsEnabled);
            MessageBus.Subscribe<BrushForegroundEnabledChangedEvent>(e => Brush.IsForegroundEnabled = e.IsEnabled);
            MessageBus.Subscribe<BrushBackgroundEnabledChangedEvent>(e => Brush.IsBackgroundEnabled = e.IsEnabled);
        }

        private void OpenDocument(Document document)
        {
            if (document.LayerCount == 0)
                throw new Exception("A document must have at least one layer.");

            Document = document;
            ActiveLayerId = Document.GetLayer(0).Id;
            Camera.ZoomToFitDocument(0);
        }

        protected override void Initialize()
        {
            // must be initialized. required by Content loading and rendering (will add itself to the Services)
            // note that MonoGame requires this to be initialized in the constructor, while WpfInterop requires it to
            // be called inside Initialize (before base.Initialize())
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);

            _mouse = new Mouse(this);
            _keyboard = new Keyboard(this);
            ConfigureEditToolInput();
            _camera = new Camera(_mouse, this);
            _documentRenderer = new DocumentRenderer(this, _camera);

            // must be called after the WpfGraphicsDeviceService instance was created
            base.Initialize();

            RenderingInitialized?.Invoke(this, EventArgs.Empty);
        }

        private void ConfigureEditToolInput()
        {
            _keyboard.KeyDown += (sender, args) => _currentEditTool?.OnKeyDown(sender, args);
            _keyboard.KeyUp += (sender, args) => _currentEditTool?.OnKeyUp(sender, args);

            _mouse.MouseMove += (sender, args) => _currentEditTool?.OnMouseMove(sender, args);
            _mouse.LeftButtonDown += (sender, args) => _currentEditTool?.OnMouseLeftButtonDown(sender, args);
            _mouse.LeftButtonUp += (sender, args) => _currentEditTool?.OnMouseLeftButtonUp(sender, args);
        }

        private void ChangeGlyph(GlyphFontViewModel glyphFontViewModel, int glyphIndex)
        {
            if (CurrentGlyphMapTexture?.GlyphFontViewModel != glyphFontViewModel)
            {
                if (!_glyphMapTextures.ContainsKey(glyphFontViewModel))
                {
                    var texture = Texture2D.FromStream(GraphicsDevice, File.OpenRead(glyphFontViewModel.Filename));
                    CurrentGlyphMapTexture = new GlyphMapTexture(glyphFontViewModel, texture, glyphFontViewModel.GlyphSize.X, glyphFontViewModel.GlyphSize.Y);
                    _glyphMapTextures.Add(glyphFontViewModel, CurrentGlyphMapTexture);
                }
                else
                {
                    CurrentGlyphMapTexture = _glyphMapTextures[glyphFontViewModel];
                }
            }
            Brush.GlyphIndex = glyphIndex;
        }

        private void ChangeBackgroundColor(GlyphColor color)
        {
            Brush.BackgroundColor = color;
        }

        private void ChangeForegroundColor(GlyphColor color)
        {
            Brush.ForegroundColor = color;
        }

        protected override void LoadContent()
        {
            _renderer = new Renderer();
            _renderer.Load(GraphicsDevice);
            _documentRenderer.Load(GraphicsDevice);
        }

        protected override void Update(GameTime time)
        {
            _keyboard.Update();
            _mouse.Update();
            _camera.Update(time);
        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(ColorUtils.FromHex(BackgroundColor).ToXnaColor());
            _camera.SetViewport(_renderer.GetViewport());
            _renderer.BeginFrame(_camera.ViewMatrix, _camera.ProjectionMatrix);

            if (Document == null)
                return;

            _documentRenderer.Render(_renderer, Document);
        }

        private void ChangeEditMode(EditMode editMode)
        {
            if (CurrentEditMode == editMode)
                return;

            switch (editMode)
            {
                case EditMode.Pencil:
                    _currentEditTool = new PencilEditTool(this);
                    break;
                case EditMode.Eraser:
                    _currentEditTool = new EraserEditTool(this);
                    break;
                case EditMode.BrushPicker:
                    break;
                default:
                    throw new NotSupportedException($"Unsupported editmode \"{editMode}\"");
            }
        }

        public VectorI GetDocumentCoordsAt(Point screenPosition)
        {
            var (x, y) = _camera.GetDocumentPosition(screenPosition);
            return new VectorI((int)(x / CurrentGlyphMapTexture.GlyphWidth), (int)(y / CurrentGlyphMapTexture.GlyphHeight));
        }

        protected override void UnloadContent()
        {
            _renderer.Unload();
            _documentRenderer.Unload();
        }

        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
            "BackgroundColor", typeof(string), typeof(DocumentControl), new PropertyMetadata(default(string)));

        /// <summary>
        /// Color of the background behind the document in this control.
        /// </summary>
        public string BackgroundColor
        {
            get => (string) GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        /// <summary>
        /// The document currenlty being shown and edited by the control.
        /// </summary>
        public Document Document { get; private set; }
        public Guid ActiveLayerId { get; private set; }
        
        /// <summary>
        /// The camera on the document.
        /// </summary>
        public ICamera Camera => _camera;

        public EditMode CurrentEditMode => _currentEditTool?.EditMode ?? EditMode.None;

        /// <summary>
        /// All settings of the Brush we draw with.
        /// </summary>
        internal GlyphBrush Brush { get; }

        /// <summary>
        /// In what font are we rendering the glyphs of the document?
        /// </summary>
        public GlyphMapTexture CurrentGlyphMapTexture { get; internal set; }
        
        /// <summary>
        /// Happens once when the control is ready for use.
        /// </summary>
        public event EventHandler RenderingInitialized;
    }
}
