namespace KamerConnect.Models;

public class ChatMessage
{
    public Guid Id { get; set; }
    public Guid MatchId { get; set; }
    public Guid senderId { get; set; }
    public string Message { get; set; }
    public DateTime SentAt { get; set; }

    public ChatMessage(Guid id, Guid matchId, Guid senderId, string message, DateTime sentAt)
    {
        Id = id;
        MatchId = matchId;
        this.senderId = senderId;
        Message = message;
        SentAt = sentAt;
    }
}