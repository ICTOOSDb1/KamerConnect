using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IChatRepository
{
    List<ChatMessage> GetChatMessages(Guid chatId);
    void CreateMessage(ChatMessage message, Guid chatId);
    void Create(List<Guid> personIds);
    public List<Chat> GetChatsFromPersonId(Guid personId);
}