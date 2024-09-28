using SERVER_SIDE.Models;
using System.Windows.Input;

namespace CHAT_APP_CLIENT.Extensions
{
    // Class that implement the IComman interface and contain the logic for handle the click command
    public class Commands : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        private Action<Member> onClick_JoinConnectedMemberChat; // Action variable to handle send Member argument to the OnClick methods in the view moedlS

        public event EventHandler? CanExecuteChanged;

        public Commands(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // ctor to handle send Member argument to the OnClick methods in the view moedl
        public Commands(Action<Member> onClick_JoinConnectedMemberChat)
        {
            this.onClick_JoinConnectedMemberChat = onClick_JoinConnectedMemberChat;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecureChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}