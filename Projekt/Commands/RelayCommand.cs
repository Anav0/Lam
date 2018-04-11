using System;
using System.Windows.Input;

namespace Projekt
{
    public class RelayCommand : ICommand
    {
        private Action<object> _Action;
        private Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> Action, Predicate<object> canExecute = null)
        {
            this._Action = Action;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute == null ? true : this._canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this._Action(parameter);
        }
    }
}
