using System;
using GlyphEdit.Bootstrapping;
using GlyphEdit.Configuration;
using GlyphEdit.Gui;

namespace GlyphEdit
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var config = ConfigLoader.Load("config.json");
            
            using (var bootstrapper = new Bootstrapper().CreateContainer(config))
            {
                var mainForm = bootstrapper.Resolve<MainForm>();
                GuiApplication.Run(mainForm, config.Theme);
            }
        }
    }
}
