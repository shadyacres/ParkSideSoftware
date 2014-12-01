using System;
using System.Diagnostics;
using System.Windows.Input;

namespace SwipeBox.Shared
{
    /// <summary>
    /// RelayCommand class for executing commands in WPF
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the RelayCommand class
        /// </summary>
        /// <param name="execute">action to execute</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initalizes a new instance of the RelayCommand class
        /// </summary>
        /// <param name="execute">Action to execute</param>
        /// <param name="canExecute">Predicate for whether the action can execute</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            _execute = execute;

           
            _canExecute = canExecute;
        }
        #endregion

        #region ICommand Members
        /// <summary>
        /// Indicates whether or not a command can execute
        /// </summary>
        /// <param name="parameter">binding parameter</param>
        /// <returns>Whether or not the the action can execute</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
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

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="parameter">binding parameter</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        #endregion
    }
        
}
