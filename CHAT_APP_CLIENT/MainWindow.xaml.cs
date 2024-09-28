using CHAT_APP_CLIENT.View_Models;
using System.Windows;
using System.Windows.Controls;

namespace CHAT_APP_CLIENT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelBase();

            if (txtJoinChat.Text == "")
            {
                txtJoinChat.Text = "Enter your name";
            }

            btnMessage.IsEnabled = !string.IsNullOrWhiteSpace(txtMessages.Text); // Disable the button that send mes
        }

        private void txtJoinChat_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtJoinChat.Text))
            {
                btnJoinChat.IsEnabled = false;
            } else
            {
                btnJoinChat.IsEnabled = true;
            }
        }

        private void txtMessages_TextChanged(object sender, TextChangedEventArgs e)
        {
            //btnMessage.IsEnabled = !string.IsNullOrWhiteSpace(txtMessages.Text); // Another way to achive the same behavior
            if (string.IsNullOrWhiteSpace(txtMessages.Text))
            {
                btnMessage.IsEnabled = false;
            } else
            {
                btnMessage.IsEnabled = true;
            }
        }

        private void txtMemberLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMemberLogin.Text))
            {
                btnConnect.IsEnabled = false;

            } else
            {
                btnConnect.IsEnabled = true;
            }
        }
    }
}