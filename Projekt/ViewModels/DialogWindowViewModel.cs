using System;
using System.Windows;
using System.Windows.Input;


namespace Projekt
{
    public class DialogWindowViewModel : BasicViewModel
    {
        public DialogWindowViewModel()
        {
            ButtonActionCommand = new RelayCommand(CloseAction);
        }

        public ICommand ButtonActionCommand { get; private set; }

        public string Message { get; set; }

        public string InsertValue { get; set; }

        public string ButtonContent { get; set; }

        public void CloseAction(Object obj)
        {
            Window window = obj as Window;
            window?.Close();
        }

    }
}
