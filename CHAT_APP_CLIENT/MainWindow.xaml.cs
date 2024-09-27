using CHAT_APP_CLIENT.View_Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            //if (txtMemberLogin.Text == "")
            //{
            //    txtMemberLogin.Text = "Enter name for write a message in your name";
            //}

            btnMessage.IsEnabled = !string.IsNullOrWhiteSpace(txtMessages.Text); // Disable the button that send mes

        }

        private void txtJoinChat_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtJoinChat.Text))
            {
                btnJoinChat.IsEnabled = false;
            }
            else
            {
                btnJoinChat.IsEnabled = true;
            }
        }

        private void txtMessages_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //btnMessage.IsEnabled = !string.IsNullOrWhiteSpace(txtMessages.Text); // Another way to achive the same behavior
            if (string.IsNullOrWhiteSpace(txtMessages.Text))
            {
                btnMessage.IsEnabled = false;
            }
            else
            {
                btnMessage.IsEnabled = true;
            }
        }
    }
}