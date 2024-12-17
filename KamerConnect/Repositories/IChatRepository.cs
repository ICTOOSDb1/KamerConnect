using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IChatRepository
{
    List<ChatMessage> GetChatMessages(Guid chatId);
    
    void CreateMessage(ChatMessage message, Guid chatId);
    List<Person> GetPersons(Guid chatId);
    Chat GetChatByMatchId(Guid matchId);
}