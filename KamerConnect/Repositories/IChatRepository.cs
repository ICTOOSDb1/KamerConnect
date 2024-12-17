using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IChatRepository
{
    List<ChatMessage> GetChatMessages(Guid chatId);
    void CreateMessage(ChatMessage message, Guid chatId);
    void CreateChat(Guid chatId);
    void AddPersonToChat(Guid chatId, Guid personId);
    void CreateChatWithPersons(List<Guid> personIds);
    List<Guid> GetPersonIdsFromChat(Guid chatId);
}