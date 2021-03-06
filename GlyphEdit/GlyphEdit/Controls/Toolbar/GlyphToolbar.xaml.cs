﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using GlyphEdit.Controls.DocumentControl;
using GlyphEdit.Controls.DocumentControl.EditTools;
using GlyphEdit.Messages;
using GlyphEdit.Messages.Commands;
using GlyphEdit.Messages.Events;
using GlyphEdit.Messaging;
using GlyphEdit.ViewModels;

namespace GlyphEdit.Controls.Toolbar
{
    /// <summary>
    /// Interaction logic for GlyphToolbar.xaml
    /// </summary>
    public partial class GlyphToolbar : UserControl
    {
        public GlyphToolbar()
        {
            InitializeComponent();

            ToolButtons = ToolStackPanel.Children.OfType<ToggleButton>().ToArray();

            MessageBus.Subscribe<EditModeChangedEvent>(e =>
            {
                foreach (var toolButton in ToolButtons)
                {
                    if (toolButton.Tag is EditMode editMode)
                    {
                        toolButton.IsChecked = editMode == e.EditMode;
                    }
                    else
                        throw new Exception("ToolButton has no EditMode tag.");
                }
            });
        }

        private void Toolbutton_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as ToggleButton);
            if (button?.Tag is EditMode editMode)
            {
                MessageBus.Publish(new ChangeEditModeCommand(editMode));
            }
            else
            {
                throw new Exception("All toolbar buttons must have an EditMode tag.");
            }
        }

        public ToggleButton[] ToolButtons { get; set; }
    }
}
