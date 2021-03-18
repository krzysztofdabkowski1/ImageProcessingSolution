using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImagePresenter.ViewModel
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }

    public abstract class AsyncCommandBase : IAsyncCommand
    {
        public abstract bool CanExecute(object parameter);
        public abstract Task ExecuteAsync(object parameter);
        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
    public  class AsyncCommand : AsyncCommandBase
    {
        private Func<Task> _execute;
        private Func<object, bool> _canExecute;

        public AsyncCommand(Func<Task> execute, Func<object, bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
            else
            {
                 _execute = execute;
            }
            
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            if (this._canExecute == null)
            {
                return true;
            }
            else
            {
                return this._canExecute(parameter);
            }
        }


        public override Task ExecuteAsync(object parameter)
        {
            return _execute();
        }

    }
}
