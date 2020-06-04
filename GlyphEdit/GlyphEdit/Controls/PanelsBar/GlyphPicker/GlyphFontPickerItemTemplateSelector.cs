using System.Windows;
using System.Windows.Controls;
using GlyphEdit.Models;

namespace GlyphEdit.Controls.PanelsBar.GlyphPicker
{
    public class GlyphFontPickerItemTemplateSelector
    : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is GlyphFont glyphFont)
            {
                var element = container as FrameworkElement;

                if (glyphFont.IsValid)
                    return element.FindResource("GlyphFontItemTemplate") as DataTemplate;
                return element.FindResource("InvalidGlyphFontItemTemplate") as DataTemplate;
            }

            return null;
        }
    }
}
