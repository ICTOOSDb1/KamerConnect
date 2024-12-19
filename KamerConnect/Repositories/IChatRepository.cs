using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IChatRepository
{
    List<ChatMessage> GetChatMessages(Guid chatId);
    void CreateMessage(ChatMessage message, Guid chatId);
    void Create(List<Guid> personIds, Guid? matchId);
    public List<Chat> GetChatsFromPerson(Guid personId);
}