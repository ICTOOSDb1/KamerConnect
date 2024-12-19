using Microsoft.AspNetCore.SignalR;

namespace KamerConnect.DataAccess.SignalR;

public class ChatHub : Hub
{
    public async Task SendMessage(Guid senderId, Guid chat, string message)
    {
        await Clients.Group(chat.ToString()).SendAsync("ReceiveMessage", senderId, message);
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