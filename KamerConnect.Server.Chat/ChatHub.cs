using KamerConnect.Models;
using Microsoft.AspNetCore.SignalR;

namespace KamerConnect.Server.Chat;

public class ChatHub : Hub
{
    public async Task SendMessage(ChatMessage chatMessage)
    {
        await Clients.All.SendAsync("ReceiveMessage", chatMessage);
    }
}