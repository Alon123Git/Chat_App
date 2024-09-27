using Microsoft.AspNetCore.SignalR.Client;
using SERVER_SIDE.Models;

namespace CHAT_APP_CLIENT.Services
{
    public class SignalRService
    {
        private readonly HubConnection _connection;
        private readonly string url = "https://localhost:5021/chathub";

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