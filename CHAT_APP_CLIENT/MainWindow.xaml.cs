using CHAT_APP_CLIENT.View_Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            DataContext = new ViewModelBase(); // Set up your ViewModel
            btnMale.IsEnabled = false;
            btnFemale.IsEnabled = false;

            if (string.IsNullOrWhiteSpace(txtJoinChat.Text))
            {
                txtJoinChat.Text = "Enter your name"; // Placeholder text
            }
        }

        private void txtJoinChat_TextChanged(object sender, TextChangedEventArgs e)
        {
            Debug.WriteLine($"Current text: '{txtJoinChat.Text}'");
            if (string.IsNullOrWhiteSpace(txtJoinChat.Text))
            {
                btnJoinChat.IsEnabled = false;
                btnMale.IsEnabled = false;
                btnFemale.IsEnabled = false;
            } else
            {
                btnMale.IsEnabled = true;
                btnFemale.IsEnabled = true;
                btnJoinChat.IsEnabled = true;
            }
        }

        private void txtMessages_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Check if the text is empty or whitespace
            if (string.IsNullOrWhiteSpace(txtMessages.Text))
            {
                btnMessage.IsEnabled = false; // Disable the button if there is no text
            } else
            {
                btnMessage.IsEnabled = true; // Enable the button if there is text
            }
            txtMessages.ScrollToEnd(); // Ensure the view scrolls to the end of the messages
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

        private void txtMessages_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Check if the Enter key is pressed without Shift
            if (e.Key == Key.Enter && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
            {
                // Send the message when Enter is pressed
                // Assuming you have a command to send the message
                var viewModel = (ViewModelBase)this.DataContext; // Adjust based on your DataContext
                viewModel.sendMessageCommand.Execute(null);

                // Prevent further processing of the Enter key
                e.Handled = true;
            }
        }

        private void MaleButtonClickEvent(object sender, RoutedEventArgs e)
        {
            btnMale.IsEnabled = false;
            btnFemale.IsEnabled = true;
        }

        private void FemaleButonClickEvent(object sender, RoutedEventArgs e)
        {
            btnFemale.IsEnabled = false;
            btnMale.IsEnabled = true;
        }
    }
}