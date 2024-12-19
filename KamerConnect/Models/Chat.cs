namespace KamerConnect.Models;

public class Chat
{
    public Guid ChatId { get; set; }
    public Guid? MatchId { get; set; }
    public List<ChatMessage> Messages { get; set; }
    public List<Person> PersonsInChat { get; set; }

    public Chat(Guid chatId, Guid? matchId, List<Person> personsInChat, List<ChatMessage> messages)
    {
        ChatId = chatId;
        MatchId = matchId;
        PersonsInChat = personsInChat;
        Messages = messages;
    }
}