using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;

namespace CHAT_APP_CLIENT
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly HttpClient _httpClient;

        public App()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5021/") // ASP.NET CORE WEB API back-end URL
            };
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Call method to reset all member login status
            ResetAllMembersLoginStatus();
        }

        private async void ResetAllMembersLoginStatus()
        {
            try
            {
                // Make API call to reset login status for all members
                var response = await _httpClient.PutAsync("api/Members/resetAllLogins", null);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("All members login status has been reset.");
                }
                else
                {
                    Console.WriteLine("Failed to reset members login status.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resetting login status: {ex.Message}");
            }
        }

    }

}
