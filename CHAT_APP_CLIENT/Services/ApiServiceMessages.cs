using SERVER_SIDE.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace CHAT_APP_CLIENT.Services
{
    public class ApiServiceMessages
    {
        private readonly HttpClient _httpClient;

        public ApiServiceMessages()
        {
            _httpClient = new HttpClient() // dependency injection for connect between the client side and the server side
            {
                BaseAddress = new Uri("https://localhost:5021/") // ASP.NET CORE WEB API back-end URL
            };
        }

        // GET MESSAGES
        public async Task<List<Message>> GetAllMessages()
        {
            var response = await _httpClient.GetAsync("api/Messages");
            response.EnsureSuccessStatusCode();
            var messages = await response.Content.ReadAsAsync<List<Message>>();
            return messages;
        }

        // ADD 1 MESSAGE
        public async Task<HttpResponseMessage> AddMessageToChatAsync(Message message)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Messages/addMessage", message);
            response.EnsureSuccessStatusCode();
            return response;
        }

        // DELETE ALL MESSAGES
        public async Task<bool> DeleteAllMessagesAsync()
        {
            try
            {
                var response = await _httpClient.DeleteAsync("api/messages/allMessages");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("All messages edleted successfully");
                    return true;
                } else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to delete messages. Status code: {response.StatusCode}, Error: {errorContent}");
                    return false;
                }
            } catch (HttpRequestException httpRequestEx)
            {
                Console.WriteLine($"HTTP Request error: {httpRequestEx.Message}"); // Log HTTP request exceptions (e.g., network issues)
                return false;
            } catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}"); // Print generic exceptions
                return false;
            }
        }
    }
}