using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messaging;
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

        // keyboard shortcut commands

        public static readonly RoutedCommand Zoom1Command = new RoutedCommand();
        public static readonly RoutedCommand Zoom2Command = new RoutedCommand();
        public static readonly RoutedCommand Zoom3Command = new RoutedCommand();
        public static readonly RoutedCommand Zoom4Command = new RoutedCommand();

        public static readonly RoutedCommand NewCommand = new RoutedCommand();
        public static readonly RoutedCommand OpenCommand = new RoutedCommand();

        public static readonly RoutedCommand SaveCommand = new RoutedCommand();
        public static readonly RoutedCommand SaveAsCommand = new RoutedCommand();

        private void Zoom1Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Publish(new ZoomToCommand(0.5f));
        }

        private void Zoom2Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Publish(new ZoomToCommand(1f));
        }

        private void Zoom3Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Publish(new ZoomToCommand(2f));
        }

        private void Zoom4Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Publish(new ZoomToCommand(4f));
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Publish(new SaveDocumentCommand());
        }

        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Publish(new SaveDocumentAsCommand());
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Publish(new NewDocumentCommand());
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Publish(new OpenDocumentCommand());
        }
    }
}
