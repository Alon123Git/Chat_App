using SERVER_SIDE.Models;
using System.Windows.Input;

namespace CHAT_APP_CLIENT.Extensions
{
    // Class that implements the ICommand interface and contains the logic for handling commands
    public class Commands : ICommand
    {
        private readonly Action _execute; // Action for commands without parameters
        private readonly Action<Member> _executeWithMember; // Action for commands with Member parameter
        private readonly Action<string> _executeWithString; // Action for commands with string parameter
        private readonly Func<bool> _canExecute; // CanExecute logic

        public event EventHandler CanExecuteChanged;

        // Constructor for Action commands
        public Commands(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Constructor for Action<Member> commands
        public Commands(Action<Member> executeWithMember, Func<bool> canExecute = null)
        {
            _executeWithMember = executeWithMember ?? throw new ArgumentNullException(nameof(executeWithMember));
            _canExecute = canExecute;
        }

        // Checks if the command can be executed
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        // Executes the command based on the type of action defined
        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute();
            } else if (_executeWithMember != null && parameter is Member member)
            {
                _executeWithMember(member);
            } else if (_executeWithString != null && parameter is string str)
            {
                _executeWithString(str);
            } else
            {
                throw new InvalidOperationException("No valid command to execute or invalid parameter type.");
            }
        }

        // To re-query CanExecute when necessary
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}