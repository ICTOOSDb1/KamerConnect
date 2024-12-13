using Microsoft.AspNetCore.SignalR;

namespace KamerConnect.Server.Chat;

public class ChatHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("MessageReceived", message);
    }
}