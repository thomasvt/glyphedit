using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GlyphEdit.Wpf
{
    public class Icon : Control
    {
        static Icon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Icon), new FrameworkPropertyMetadata(typeof(Icon)));
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(double), typeof(Icon), new PropertyMetadata(32d, (o, args) =>
            {
                var icon = (Icon)o;
                var size = (double)args.NewValue;
                icon.Width = size;
                icon.Height = size;
            }));

        public double Size
        {
            get { return (float)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty GeometryProperty = DependencyProperty.Register(
            "Geometry", typeof(Geometry), typeof(Icon), new PropertyMetadata(default(Geometry)));

        public Geometry Geometry
        {
            get { return (Geometry)GetValue(GeometryProperty); }
            set { SetValue(GeometryProperty, value); }
        }

        public static readonly DependencyProperty FillProperty =
                IconHelper.FillProperty.AddOwner(typeof(Icon), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

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

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof(double), typeof(Icon), new PropertyMetadata(default(double)));

        public double StrokeThickness
        {
            get { return (double) GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof(Brush), typeof(Icon), new PropertyMetadata(default(Brush)));

        public Brush Stroke
        {
            get { return (Brush) GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
    }
}
