﻿using System;
using System.Windows.Input;

namespace Projekt.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _action;

        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> action, Predicate<object> canExecute = null)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public RelayCommand(Action action, Predicate<object> canExecute = null)
        {
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}