using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows;

namespace MonoGame.Framework.WpfInterop
{
    /// <summary>
    /// The <see cref="Microsoft.Xna.Framework.Content.ContentManager"/> needs a <see cref="IGraphicsDeviceService"/> to be in the <see cref="System.ComponentModel.Design.IServiceContainer"/>. This class fulfills this purpose.
    /// </summary>
    public class WpfGraphicsDeviceService : IGraphicsDeviceService, IGraphicsDeviceManager
    {
        internal const int MsaaSampleLimit = 32;

        private readonly WpfGame _host;

        #region Constructors

        /// <summary>
        /// Create a new instance of the dummy. The constructor will autom. add the instance itself to the <see cref="D3D11Host.Services"/> container of <see cref="host"/>.
        /// </summary>
        /// <param name="host"></param>
        public WpfGraphicsDeviceService(WpfGame host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));

            if (host.Services.GetService(typeof(IGraphicsDeviceService)) != null)
                throw new NotSupportedException("A graphics device service is already registered.");

            if (host.GraphicsDevice == null)
                throw new ArgumentException("Provided host graphics device is null.");

            GraphicsDevice = host.GraphicsDevice;
            _host.GraphicsDevice.DeviceReset += (sender, args) => DeviceReset?.Invoke(this, args);
            _host.GraphicsDevice.DeviceResetting += (sender, args) => DeviceResetting?.Invoke(this, args);

            host.Services.AddService(typeof(IGraphicsDeviceService), this);
            host.Services.AddService(typeof(IGraphicsDeviceManager), this);
        }

        #endregion

        #region Events

        /// <inheritdoc />
        public event EventHandler<EventArgs> DeviceCreated;

        /// <inheritdoc />
        public event EventHandler<EventArgs> DeviceDisposing;

        /// <inheritdoc />
        public event EventHandler<EventArgs> DeviceReset;

        /// <inheritdoc />
        public event EventHandler<EventArgs> DeviceResetting;

        #endregion

        #region Properties

        public GraphicsDevice GraphicsDevice { get; }

        public bool PreferMultiSampling { get; set; }
        
        /// <summary>
        /// When called returns the system Dpi scaling factor.
        /// The scaling factor may be different between different monitors.
        /// This value will always return the value based on the monitor where the attached gamecontrol is positioned.
        /// </summary>
        public double SystemDpiScalingFactor => PresentationSource.FromVisual(_host).CompositionTarget.TransformToDevice.M11;

        public int PreferredBackBufferWidth => (int)_host.ActualWidth;

        public int PreferredBackBufferHeight => (int)_host.ActualHeight;

        #endregion

        #region Methods

        public bool BeginDraw()
        {
            return true;
        }

        public void CreateDevice()
        {
            ApplyChanges();
            DeviceCreated?.Invoke(this, EventArgs.Empty);
        }

        public void EndDraw()
        {

        }

        public void ApplyChanges()
        {
            var size = _host.CalculateNeededBackBufferSize();
            var width = Math.Max((int)size.Width, 1);
            var height = Math.Max((int)size.Height, 1);
            var pp = new PresentationParameters
            {
                // set to windows limit, if gpu doesn't support it, monogame will autom. scale it down to the next supported level
                MultiSampleCount = PreferMultiSampling ? MsaaSampleLimit : 0,
                BackBufferWidth = width,
                BackBufferHeight = height,
                DeviceWindowHandle = IntPtr.Zero
            };
            // would be so easy to just call reset. but for some reason monogame doesn't want the WindowHandle to be null on reset (but it's totally fine to be null on create)
            // GraphicsDevice.Reset(pp);
            DeviceDisposing?.Invoke(this, EventArgs.Empty);
            // manually work around it by telling our base implementation to handle the changes
            _host.RecreateGraphicsDevice(pp);
        }

        #endregion
    }
}
