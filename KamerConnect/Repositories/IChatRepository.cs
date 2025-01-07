using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IChatRepository
{
    List<ChatMessage> GetChatMessages(Guid chatId);

    void CreateMessage(ChatMessage message, Guid chatId);
    public void Create(Chat chat);
    public List<Chat> GetChatsFromPerson(Guid personId);
}