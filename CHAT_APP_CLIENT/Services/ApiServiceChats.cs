using SERVER_SIDE.Models;
using System.Net.Http;

namespace CHAT_APP_CLIENT.Services
{
    public class ApiServiceChats
    {
        private readonly HttpClient _httpClient;  // dependency injection for connect between the client side and the server side

        public ApiServiceChats()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:5021/") // ASP.NET CORE WEB API back-end URL
            };
        }

        // GET CHATS
        public async Task<List<Chat>> GetAllChatsAsync()
        {
            var response = await _httpClient.GetAsync("api/Chats");
            response.EnsureSuccessStatusCode();
            var chats = await response.Content.ReadAsAsync<List<Chat>>();
            return chats;
        }
        
        // ADD 1 CHAT
        public async Task<HttpResponseMessage> AddChatAsync(Chat chat)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Chats/addChat", chat);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}