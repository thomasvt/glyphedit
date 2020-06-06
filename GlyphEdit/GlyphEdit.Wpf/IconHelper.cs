using System;
using System.Windows;
using System.Windows.Media;

namespace GlyphEdit.Wpf
{
    public class IconHelper
    {
        public static readonly DependencyProperty FillProperty = DependencyProperty.RegisterAttached(
            "Fill",
            typeof(Brush),
            typeof(IconHelper),
            new FrameworkPropertyMetadata(
                default(Brush),
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender |
                FrameworkPropertyMetadataOptions.Inherits));

        public static void SetFill(DependencyObject element, Brush value)
        {
            if (element == null)
            {
                throw new ArgumentNullException(element.DependencyObjectType.Name);
            }

            element.SetValue(FillProperty, value);
        }

        public static Brush GetFill(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(element.DependencyObjectType.Name);
            }

            return (Brush)element.GetValue(FillProperty);
        }
    }
}
