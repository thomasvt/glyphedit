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
            // this event comes from the D3DHost gameloop thread, so: sync to UI thread:
            Dispatcher.InvokeAsync(() => EditorViewModel.Current.OnLoaded());
        }
    }
}
