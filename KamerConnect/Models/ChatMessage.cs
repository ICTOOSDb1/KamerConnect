namespace KamerConnect.Models;

public class ChatMessage
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public string Message { get; set; }
    public DateTime SendAt { get; set; }

    public ChatMessage(Guid id, Guid chatId, Guid senderId, string message, DateTime sendAt)
    {
        Id = id;
        ChatId = chatId;
        SenderId = senderId;
        Message = message;
        SendAt = sendAt;
    }
}