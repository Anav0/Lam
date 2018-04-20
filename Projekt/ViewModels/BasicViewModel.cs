using System.ComponentModel;
using PropertyChanged;

namespace Projekt.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class BasicViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}