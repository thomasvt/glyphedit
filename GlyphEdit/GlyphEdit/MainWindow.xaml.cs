using System;
using System.Windows;
using GlyphEdit.ViewModels;

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

#if !DEBUG
            WindowState = WindowState.Maximized;
#endif
        }

        private void DocumentViewer_OnRenderingInitialized(object sender, EventArgs e)
        {
            // this event comes from the D3DHost gameloop thread, so: sync to UI thread:
            Dispatcher.InvokeAsync(() => EditorViewModel.Current.OnLoaded());
        }
    }
}
