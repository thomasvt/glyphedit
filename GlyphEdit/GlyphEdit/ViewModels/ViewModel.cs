using System.ComponentModel;
using System.Runtime.CompilerServices;
using GlyphEdit.Annotations;

namespace GlyphEdit.ViewModels
{
    public abstract class ViewModel
    : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
