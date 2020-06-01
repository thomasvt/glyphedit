using GlyphEdit.Gui.Controls;

namespace GlyphEdit.Gui
{
    public static class GuiApplication
    {
        public static void Run(Form mainForm, GuiTheme theme)
        {
            using (var guiApp = new GuiApp(mainForm, theme))
            {
                guiApp.Run();
            }
        }
    }
}
