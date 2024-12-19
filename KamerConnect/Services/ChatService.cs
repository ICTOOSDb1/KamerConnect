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

    public void Create(List<Guid> personIds, Guid? matchId)
    {
        _chatRepository.Create(personIds, matchId);
    }
    
    public List<Chat> GetChatsFromPerson(List<Chat> chats)
    {
        return _chatRepository.GetChatsFromPerson(chats);
    }
}