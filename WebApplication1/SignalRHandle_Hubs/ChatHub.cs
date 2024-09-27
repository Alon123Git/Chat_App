using Microsoft.AspNetCore.SignalR;

namespace SERVER_SIDE.SignalRHandle_Hubs
{
    /// <summary>
    /// Class handle SignalR initialization
    /// </summary>
    public class ChatHub : Hub
    {
        // Methods for be able to connect with the client side

        //sends a message to all connected clients, using Clients.All
        public async Task SendMessage(string user, string message)
            => await Clients.All.SendAsync("ReceiveMessage", user, message);

        //sends a message back to the caller, using Clients.Caller
        public async Task SendMessageToCaller(string user, string message)
            => await Clients.Caller.SendAsync("ReceiveMessage", user, message);

        // sends a message to all clients in the SignalR Users group
        public async Task SendMessageToGroup(string user, string message)
            => await Clients.Group("SignalR Users").SendAsync("ReceiveMessage", user, message);

        // Wait for client response
        public async Task<string> WaitForMessage(string connectionId)
        {
            return await Task.FromResult("Waiting for client response...");
        }
    }
}