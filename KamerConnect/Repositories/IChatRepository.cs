using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IChatRepository
{
    List<ChatMessage> getChatMessages(Guid matchId, Guid personId);
    
    void SendMessage(ChatMessage message);
    
}