﻿
using Microsoft.AspNetCore.SignalR;


namespace KamerConnect.Service.Chat;

public class ChatHub : Hub
{
    public async Task SendMessage(Guid senderId, Guid chat, string message)
    {
        await Clients.Group(chat.ToString()).SendAsync("ReceiveMessage", senderId, message);
    }
    public async Task SendCard(Guid senderId, Guid chat)
    {
        await Clients.Group(chat.ToString()).SendAsync("ReceiveCard", senderId);
    }

    public override async Task OnConnectedAsync()
    {

        var chatId = Context.GetHttpContext()?.Request.Query["chatId"].ToString();

        if (!string.IsNullOrEmpty(chatId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }
        else
        {
            throw new InvalidOperationException("Chat ID is required to join the group.");
        }

        await base.OnConnectedAsync();
    }
}