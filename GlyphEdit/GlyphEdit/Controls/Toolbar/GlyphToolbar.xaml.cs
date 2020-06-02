using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using GlyphEdit.Controls.DocumentView;
using GlyphEdit.Messaging;

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

            ToolButtons = new[]
            {
                PencilToolButton,
                EraserToolButton
            };
        }

        private void Toolbutton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var toolButton in ToolButtons)
            {
                var isClickedTool = toolButton == sender;
                toolButton.IsChecked = isClickedTool;
                if (isClickedTool)
                {
                    if (toolButton.Tag is EditMode editMode)
                        MessageBus.Publish(new ChangeEditModeCommand(editMode));
                    else
                        throw new Exception("ToolButton has no EditMode tag.");
                }
            }
        }

        public ToggleButton[] ToolButtons { get; set; }
    }
}
