using GlyphEdit.Configuration;
using Hellhole.FastContainer;

namespace GlyphEdit.Bootstrapping
{
    public class Bootstrapper
    {
        public Container CreateContainer(Config config)
        {
            var builder = new ContainerBuilder();
            builder.Register<MainForm>(LifetimeScope.Singleton);
            return builder.Build();
        }
    }
}
