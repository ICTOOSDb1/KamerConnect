using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IChatRepository
{
    List<ChatMessage> GetChatMessages(Guid chatId);

    void CreateMessage(ChatMessage message, Guid chatId);
    public void Create(List<Chat> chats);
    public List<Chat> GetChatsFromPerson(Guid personId);
}