using System.Windows;
using GlyphEdit.Controls.DocumentView.Input;
using GlyphEdit.Controls.DocumentView.Model;
using GlyphEdit.Controls.DocumentView.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;

namespace GlyphEdit.Controls.DocumentView
{
    public class GlyphDocumentViewer : WpfGame
    {
        private IGraphicsDeviceService _graphicsDeviceManager;
        private GlyphMouse _mouse;
        private GlyphKeyboard _keyboard;
        private readonly DocumentControl _documentControl;
        private Renderer _renderer;

        public GlyphDocumentViewer()
        {
            _documentControl = new DocumentControl();
        }

        protected override void Initialize()
        {
            // must be initialized. required by Content loading and rendering (will add itself to the Services)
            // note that MonoGame requires this to be initialized in the constructor, while WpfInterop requires it to
            // be called inside Initialize (before base.Initialize())
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);

            _mouse = new GlyphMouse(this);
            _keyboard = new GlyphKeyboard(this);

            ViewSettings = DocumentViewSettings.Default;

            // must be called after the WpfGraphicsDeviceService instance was created
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Camera = new Camera(_mouse);

            _renderer = new Renderer();
            _renderer.Load(GraphicsDevice);

            _documentControl.Load(GraphicsDevice);
        }

        protected override void Update(GameTime time)
        {
            _mouse.Update();
        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(Colors.FromHex(BackgroundColor));
            _renderer.BeginFrame();

            _renderer.SetCameraPosition(Camera.Position);
            _documentControl.Render(_renderer);
        }

        protected override void UnloadContent()
        {
            _renderer.Unload();
            _documentControl.Unload();
        }

        public Document Document 
        { 
            get => _documentControl.Document;
            set => _documentControl.Document = value;
        }

        public DocumentViewSettings ViewSettings
        {
            get => _documentControl.ViewSettings;
            set => _documentControl.ViewSettings = value;
        }

        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
            "BackgroundColor", typeof(string), typeof(GlyphDocumentViewer), new PropertyMetadata(default(string)));

        public string BackgroundColor
        {
            get => (string) GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public ICamera Camera { get; private set; }
    }
}
