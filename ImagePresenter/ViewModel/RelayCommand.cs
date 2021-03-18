using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ImagePresenter.ViewModel
{
    class RelayCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        public RelayCommand(Action<object>execute, Func<object, bool> canExecute = null)
        {
            if(execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
            else
            this._execute = execute;
            this._canExecute = canExecute;
        }
 
        public bool CanExecute(object parameter)
        {
            if(this._canExecute == null)
            {
                return true;
            }
            else
            {
                return this._canExecute(parameter);
            }
        }
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
        public void Execute(object parameter)
        {
            this._execute(parameter); 
        }

    }
}
