using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IChatRepository
{
    List<ChatMessage> GetChatMessages(Guid chatId);
    
    void CreateMessage(ChatMessage message);
}