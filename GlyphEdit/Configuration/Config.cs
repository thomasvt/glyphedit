using GlyphEdit.Gui;

namespace GlyphEdit.Configuration
{
    public class Config
    {
        public float UiZoom { get; set; }
        public GuiTheme Theme { get; set; }

        public static Config GetDefault()
        {
            return new Config
            {
                UiZoom = 1f,
                Theme = new GuiTheme
                {
                    FontName = "font1"
                }
            };
        }
    }
}
