using SERVER_SIDE.Models;
using System.Windows.Input;

namespace CHAT_APP_CLIENT.Extensions
{
    // Class that implements the ICommand interface and contains the logic for handling commands
    public class Commands : ICommand
    {
        private readonly Action _execute; // Action for commands without parameters
        private readonly Func<bool> _canExecute; // CanExecute logic

        public event EventHandler CanExecuteChanged;

        // Constructor for Action commands (no parameters)
        public Commands(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            // Execute the Action without parameters
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}