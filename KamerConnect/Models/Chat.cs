namespace KamerConnect.Models;

public class Chat
{
    public Guid ChatId { get; set; }
    public Guid matchId { get; set; }
    public List<ChatMessage> messages { get; set; }

    public Chat(Guid chatId, Guid matchId)
    {
        ChatId = chatId;
        this.matchId = matchId;
    }
}