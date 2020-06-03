using System;
using System.Windows;
using GlyphEdit.Controls.DocumentView.Input;
using GlyphEdit.Controls.DocumentView.Rendering;
using GlyphEdit.Messages;
using GlyphEdit.Messaging;
using GlyphEdit.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using Point = Microsoft.Xna.Framework.Point;

namespace GlyphEdit.Controls.DocumentView
{
    public class DocumentControl : WpfGame
    {
        private EditTool _currentEditTool;
        private IGraphicsDeviceService _graphicsDeviceManager;
        private GlyphMouse _mouse;
        private GlyphKeyboard _keyboard;
        private DocumentRenderer _documentRenderer;
        private Renderer _renderer;
        private Camera _camera;

        public DocumentControl()
        {
            MessageBus.Subscribe<DocumentOpenedEvent>(e =>
            {
                Document = e.Document;
                Camera.Reset();
            });
            MessageBus.Subscribe<EditModeChangedEvent>(e => ChangeEditMode(e.EditMode));
        }

        protected override void Initialize()
        {
            // must be initialized. required by Content loading and rendering (will add itself to the Services)
            // note that MonoGame requires this to be initialized in the constructor, while WpfInterop requires it to
            // be called inside Initialize (before base.Initialize())
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);

            _mouse = new GlyphMouse(this);
            _keyboard = new GlyphKeyboard(this);
            _camera = new Camera(_mouse, this);
            _documentRenderer = new DocumentRenderer(_camera);

            ViewSettings = DocumentViewSettings.Default;

            // must be called after the WpfGraphicsDeviceService instance was created
            base.Initialize();

            RenderingInitialized?.Invoke(this, EventArgs.Empty);
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
        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(Colors.FromHex(BackgroundColor));
            _camera.SetViewport(_renderer.GetViewport());
            _renderer.BeginFrame(_camera.ViewMatrix, _camera.ProjectionMatrix);

            if (Document == null)
                return;

            _documentRenderer.Render(_renderer, Document, ViewSettings);
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
            var glyphSize = _documentRenderer.GetGlyphRenderSize();
            return new Point((int)(x / glyphSize.X), (int)(y / glyphSize.Y));
        }

        protected override void UnloadContent()
        {
            _renderer.Unload();
            _documentRenderer.Unload();
        }


        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
            "BackgroundColor", typeof(string), typeof(DocumentControl), new PropertyMetadata(default(string)));

        public string BackgroundColor
        {
            get => (string) GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public DocumentViewSettings ViewSettings { get; set; }

        public Document Document { get; set; }

        public ICamera Camera => _camera;

        public EditMode CurrentEditMode => _currentEditTool?.EditMode ?? EditMode.None;

        public event EventHandler RenderingInitialized;
    }
}
