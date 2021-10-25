using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RemoteHealthcare_Client
{
    /// <summary>
    /// Class represents a implementation of ICommand, used to call mehtods on databinding
    /// </summary>
    class GeneralCommand : ICommand
    {
        /// <summary>
        /// The event handles that is called when canExecute changed
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        readonly Action<object> ExecuteCommand;
        readonly Predicate<object> CanExecuteAction;

        /// <summary>
        /// Constructor for General command, canExecute is set to null
        /// </summary>
        /// <param name="execute">The action performed when method is called</param>
        public GeneralCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Main constructor for GeneralCommand
        /// </summary>
        /// <param name="execute">The action performed when method is called</param>
        /// <param name="canExecute">The function that determens if the action is allowed</param>
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
