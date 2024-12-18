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
    public void CreateMessage(ChatMessage message, Guid chatId)
    { 
        _chatRepository.CreateMessage(message, chatId);
    }

    public void Create(List<Guid> personIds)
    {
        _chatRepository.Create(personIds);
    }

    public List<Chat> GetChatsFromPersonId(Guid personId)
    {
        return _chatRepository.GetChatsFromPersonId(personId);
    }
}