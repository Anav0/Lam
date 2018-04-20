using System.Windows;
using System.Windows.Input;
using Projekt.Commands;

namespace Projekt.ViewModels
{
    public class DialogWindowViewModel : BasicViewModel
    {
        public DialogWindowViewModel()
        {
            ButtonActionCommand = new RelayCommand(CloseAction);
        }

        public ICommand ButtonActionCommand { get; }

        public string Message { get; set; }

        public string InsertValue { get; set; }

        public string ButtonContent { get; set; }

        public void CloseAction(object obj)
        {
            var window = obj as Window;
            window?.Close();
        }
    }
}