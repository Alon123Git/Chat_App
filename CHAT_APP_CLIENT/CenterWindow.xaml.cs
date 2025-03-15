using CHAT_APP_CLIENT.View_Models;
using System.Windows;

namespace CHAT_APP_CLIENT
{
    /// <summary>
    /// Interaction logic for CenterWindow.xaml
    /// </summary>
    public partial class CenterWindow : Window
    {
        public CenterWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelCenter(); // Set up your ViewModel
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            this.Close();
            mw.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            InitialWindow iw = new InitialWindow();
            this.Close();
            iw.Show();
        }
    }
}