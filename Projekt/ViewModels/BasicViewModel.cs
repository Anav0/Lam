using Projekt.Windows;
using PropertyChanged;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Projekt
{
    [AddINotifyPropertyChangedInterfaceAttribute]

    public class BasicViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public BasicViewModel()
        {

        }

    }

}

