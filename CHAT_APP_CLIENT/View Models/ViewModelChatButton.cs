using CHAT_APP_CLIENT.Extensions;
using SERVER_SIDE.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CHAT_APP_CLIENT.View_Models
{
    public class ViewModelChatButton
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string ChatName { get; }
        private string _displayChatTitle = string.Empty;
        public int ChatId { get; }
        public ICommand NavigateCommand { get; }
        public ICommand ChatTitleName { get; private set; }

        // Parameterless constructor for XAML use
        public ViewModelChatButton()
        {
            // Default values for properties, or leave them as is
            ChatName = "Default Chat Name";
            ChatId = 0;
            NavigateCommand = new Commands(NavigateToChat);
        }

        public ViewModelChatButton(Chat chat) : this() // Use the parameterless constructor
        {
            ChatName = chat._name;
            ChatId = chat._id;
        }

        //public string DisplayChatTitle
        //{
        //    get { return _displayChatTitle; }
        //    set
        //    {
        //        if (_displayChatTitle != value)
        //        {
        //            _displayChatTitle = value;
        //            OnPropertyChanged(nameof(DisplayChatTitle));
        //        }
        //    }
        //}

        private void NavigateToChat()
        {
            // ✅ Retrieve the existing ViewModel instance
            var mainWindowViewModel = new ViewModelBase();
            mainWindowViewModel.ChatTitle = this.ChatName; // ✅ Set the Chat Name
            // ✅ Pass the ViewModel when creating the new window
            var mainWindow = new MainWindow(mainWindowViewModel);
            mainWindow.Show();

            // Close the CenterWindow
            var chatWindow = Application.Current.Windows.OfType<CenterWindow>().FirstOrDefault();
            if (chatWindow != null)
            {
                chatWindow.Close();
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
