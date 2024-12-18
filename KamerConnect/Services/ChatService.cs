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

    public List<Person> GetPersons(Guid chatId)
    {
       return _chatRepository.GetPersons(chatId);
    }

    public Chat GetChatByMatchId(Guid matchId)
    {
        return _chatRepository.GetChatByMatchId(matchId);
    }
}