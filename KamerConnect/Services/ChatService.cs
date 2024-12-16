using KamerConnect.Models;
using KamerConnect.Repositories;

namespace KamerConnect.Services;

public class ChatService
{
    private IChatRepository _chatRepository;

    public ChatService(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public List<ChatMessage> getChatMessages(Guid matchId, Guid personId)
    {
        return _chatRepository.getChatMessages(matchId, personId);
    }

    public void sendMessage(ChatMessage message)
    { 
        _chatRepository.SendMessage(message);
    }
}