using System;
using System.Windows;
using GlyphEdit.ViewModel;

namespace GlyphEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void DocumentViewer_OnRenderingInitialized(object sender, EventArgs e)
        {
            EditorViewModel.Current.OnLoaded();
        }
    }
}
