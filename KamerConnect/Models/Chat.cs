using System.Collections.ObjectModel;

namespace KamerConnect.Models;

public class Chat
{
    public Guid ChatId { get; set; }
    public Guid matchId { get; set; }
    public ObservableCollection<ChatMessage> messages { get; set; } = new();

    public Chat(Guid chatId, Guid matchId)
    {
        ChatId = chatId;
        this.matchId = matchId;
    }
}