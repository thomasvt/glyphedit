using System.Windows.Controls;
using System.Windows.Input;

namespace GlyphEdit.Wpf
{
    /// <summary>
    /// Pressing enter is like pressing tab.
    /// </summary>
    public class GlyphTextBox : TextBox
    {
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Enter)
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}
