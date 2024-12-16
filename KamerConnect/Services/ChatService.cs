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

    public List<ChatMessage> GetChatMessages(Guid chatId)
    {
        return _chatRepository.GetChatMessages(chatId);
    }
    public void CreateMessage(ChatMessage message)
    { 
        _chatRepository.CreateMessage(message);
    }   
}