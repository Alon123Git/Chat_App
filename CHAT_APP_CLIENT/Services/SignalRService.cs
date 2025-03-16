using Microsoft.AspNetCore.SignalR.Client;
using SERVER_SIDE.Models;

namespace CHAT_APP_CLIENT.Services
{
    // class that hanle the SignalR operation
    public class SignalRService
    {
        private readonly HubConnection _connection; // Dependency injection for connect the SignalR from the server side
        private readonly string url = "https://localhost:5021/chathub"; // URL of the server side

        public SignalRService()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();
        }
        
        public async Task StartAsync()
        {
            await _connection.StartAsync();
        }

        public async Task SendMessageAsync(string userName, Message message)
        {
            await _connection.InvokeAsync("SendMessage", userName, message);
        }

        public void OnMessageReceived(Action<string, Message> handler)
        {
            _connection.On("ReceiveMessage", handler);
        }
    }
}