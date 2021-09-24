using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RemoteHealthcare_Client
{
    class GeneralCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        readonly Action<object> ExecuteCommand;
        readonly Predicate<object> CanExecuteAction;

        public GeneralCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public GeneralCommand(Action<object> execute, Predicate<object> canExecute)
        {

            ExecuteCommand = execute;
            CanExecuteAction = canExecute;
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecuteAction == null ? true : CanExecuteAction(parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteCommand(parameter);
        }
    }
}
