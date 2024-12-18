namespace KamerConnect.Models;

public class Chat
{
    public Guid ChatId { get; set; }
    public Guid? matchId { get; set; }
    public List<ChatMessage> messages { get; set; }
    
    List<Person> PersonsInChat { get; set; }

    public Chat(Guid chatId, Guid? matchId, List<Person> personsInChat, List<ChatMessage> messages)
    {
        ChatId = chatId;
        this.matchId = matchId;
        this.PersonsInChat = personsInChat;
        this.messages = messages;
    }
}