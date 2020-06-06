﻿using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;
using GlyphEdit.ViewModels;

namespace GlyphEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private bool _isExiting;
        private bool _canUndo;
        private bool _canRedo;
        private string _documentFilename;

        public MainWindow()
        {
            InitializeComponent();

#if !DEBUG
            WindowState = WindowState.Maximized;
#endif
            MessageBus.Subscribe<DocumentOpenedEvent>(e =>
            {
                _documentFilename = e.Document.Filename;
                UpdateWindowTitle();
            });
            MessageBus.Subscribe<DocumentFilenameChangedEvent>(e =>
            {
                _documentFilename = e.Filename;
                UpdateWindowTitle();
            });
            MessageBus.Subscribe<UndoStackStateChangedEvent>(e =>
            {
                _canUndo = e.CanUndo;
                _canRedo = e.CanRedo;
                UpdateWindowTitle();
            });
            MessageBus.Subscribe<ExitApplicationEvent>(e =>
            {
                // shutdown initiated from ViewModel
                _isExiting = true;
                Application.Current.Shutdown(0); // healthy shutdown
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // our workflow passes this method for 2 reasons: viewmodel decides to exit, or user presses Close button in window chrome.
            if (_isExiting) 
            {
                // viewmodel decided to exit app, do that.
                e.Cancel = false;
                return;
            }
            // user pressed the Close button on the window: initiate close workflow
            e.Cancel = true; // cancel the normal workflow, we go through messagebus command and event
            PostExitApplicationCommand(); // messagebus is synchronous, so we'll be shutting down before this method runs out anyway.
        }

        private static void PostExitApplicationCommand()
        {
            MessageBus.Publish(new ExitApplicationCommand());
        }

        private void UpdateWindowTitle()
        {
            var version = Assembly.GetEntryAssembly().GetName().Version;
            if (_documentFilename != null)
            {
                if (_canUndo)
                    Title = $"*{Path.GetFileName(_documentFilename)} - GlyphEdit v{version.Major}.{version.Minor}";
                else
                    Title = $"{Path.GetFileName(_documentFilename)} - GlyphEdit v{version.Major}.{version.Minor}";
            }
            else
            {
                Title = $"<new project> - GlyphEdit v{version.Major}.{version.Minor}";
            }
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
        public static readonly RoutedCommand Zoom0Command = new RoutedCommand();

        public static readonly RoutedCommand NewCommand = new RoutedCommand();
        public static readonly RoutedCommand OpenCommand = new RoutedCommand();

        public static readonly RoutedCommand SaveCommand = new RoutedCommand();
        public static readonly RoutedCommand SaveAsCommand = new RoutedCommand();

        public static readonly RoutedCommand UndoCommand = new RoutedCommand();
        public static readonly RoutedCommand RedoCommand = new RoutedCommand();

        public static readonly RoutedCommand ExitCommand = new RoutedCommand();
        

        private void Zoom1Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Publish(new ZoomToCommand(1f));
        }

        private void Zoom2Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Publish(new ZoomToCommand(2f));
        }

        private void Zoom3Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Publish(new ZoomToCommand(4f));
        }

        private void Zoom0Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Publish(new ZoomToFitCommand());
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

        private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Publish(new UndoCommand());
        }

        private void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Publish(new RedoCommand());
        }

        private void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _canUndo;
        }

        private void RedoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _canRedo;
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PostExitApplicationCommand();
        }
    }
}
