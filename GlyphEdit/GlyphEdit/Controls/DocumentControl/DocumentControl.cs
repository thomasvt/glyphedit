using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using GlyphEdit.Controls.DocumentView;
using GlyphEdit.Controls.DocumentView.Input;
using GlyphEdit.Controls.DocumentView.Rendering;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;
using GlyphEdit.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using Point = Microsoft.Xna.Framework.Point;

namespace GlyphEdit.Controls.DocumentControl
{
    public class DocumentControl : WpfGame
    {
        private EditTool _currentEditTool;
        private IGraphicsDeviceService _graphicsDeviceManager;
        private WpfMouse _mouse;
        private WpfKeyboard _keyboard;
        private DocumentRenderer _documentRenderer;
        private Renderer _renderer;
        private Camera _camera;
        private readonly Dictionary<GlyphFont, GlyphMapTexture> _glyphMapTextures;
        
        public DocumentControl()
        {
            _glyphMapTextures = new Dictionary<GlyphFont, GlyphMapTexture>();
            Brush = new GlyphBrush();

            MessageBus.Subscribe<DocumentOpenedEvent>(e =>
            {
                Document = e.Document;
                Camera.Reset();
            });
            MessageBus.Subscribe<EditModeChangedEvent>(e => ChangeEditMode(e.EditMode));
            MessageBus.Subscribe<GlyphChangedEvent>(e => ChangeGlyph(e.NewGlyphFont, e.NewGlyphIndex));
            MessageBus.Subscribe<ForegroundColorChangedEvent>(w => ChangeForegroundColor(w.Color));
            MessageBus.Subscribe<BackgroundColorChangedEvent>(w => ChangeBackgroundColor(w.Color));
            MessageBus.Subscribe<BrushGlyphEnabledChangedEvent>(e => Brush.IsGlyphEnabled = e.IsEnabled);
            MessageBus.Subscribe<BrushForegroundEnabledChangedEvent>(e => Brush.IsForegroundEnabled = e.IsEnabled);
            MessageBus.Subscribe<BrushBackgroundEnabledChangedEvent>(e => Brush.IsBackgroundEnabled = e.IsEnabled);
            MessageBus.Subscribe<ZoomToCommand>(c => _camera.ZoomSmoothTo(c.Percentage, 0.2f));
        }

        protected override void Initialize()
        {
            // must be initialized. required by Content loading and rendering (will add itself to the Services)
            // note that MonoGame requires this to be initialized in the constructor, while WpfInterop requires it to
            // be called inside Initialize (before base.Initialize())
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);

            _mouse = new WpfMouse(this);
            _keyboard = new WpfKeyboard(this);
            _camera = new Camera(_mouse, this);
            _documentRenderer = new DocumentRenderer(this, _camera);

            // must be called after the WpfGraphicsDeviceService instance was created
            base.Initialize();

            RenderingInitialized?.Invoke(this, EventArgs.Empty);
        }

        private void ChangeGlyph(GlyphFont glyphFont, int glyphIndex)
        {
            if (CurrentGlyphMapTexture?.GlyphFont != glyphFont)
            {
                if (!_glyphMapTextures.ContainsKey(glyphFont))
                {
                    var texture = Texture2D.FromStream(GraphicsDevice, File.OpenRead(glyphFont.Filename));
                    CurrentGlyphMapTexture = new GlyphMapTexture(glyphFont, texture, glyphFont.GlyphSize.X, glyphFont.GlyphSize.Y);
                    _glyphMapTextures.Add(glyphFont, CurrentGlyphMapTexture);
                }
                else
                {
                    CurrentGlyphMapTexture = _glyphMapTextures[glyphFont];
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
            _mouse.Update();
            _camera.Update(time);
        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(Colors.FromHex(BackgroundColor));
            _camera.SetViewport(_renderer.GetViewport());
            _renderer.BeginFrame(_camera.ViewMatrix, _camera.ProjectionMatrix);

            if (Document == null)
                return;

            _documentRenderer.Render(_renderer, Document);
        }

        public void ChangeEditMode(EditMode editMode)
        {
            if (CurrentEditMode == editMode)
                return;

            switch (editMode)
            {
                case EditMode.Pencil:
                    _currentEditTool = new PencilEditTool(this, _mouse);
                    break;
                case EditMode.Eraser:
                    break;
                default:
                    throw new NotSupportedException($"Unsupported editmode \"{editMode}\"");
            }
        }

        public Point GetDocumentCoordsAt(Point screenPosition)
        {
            var (x, y) = _camera.GetDocumentPosition(screenPosition);
            return new Point((int)(x / CurrentGlyphMapTexture.GlyphWidth), (int)(y / CurrentGlyphMapTexture.GlyphHeight));
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
        public Document Document { get; set; }

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

        public event EventHandler RenderingInitialized;
    }
}
