using KamerConnect.DataAccess.Postgres.Repositories;
using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Services;
using Moq;
using Npgsql;

namespace KamerConnect.UnitTests.Services;

[TestFixture]
public class ChatRepositoryTests
{
    private Mock<IChatRepository> _mockRepository;
    private ChatService _chatService;
    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IChatRepository>();
        _chatService = new ChatService(_mockRepository.Object);
    }
    [Test]
    public void Create_WhenCalled_createMessage()
    {
        // Arrange
        var newMessage = new ChatMessage(Guid.NewGuid(), Guid.NewGuid(), "test", DateTime.Now);
        var chat = new Chat(Guid.NewGuid(), Guid.NewGuid());
        _mockRepository.Setup(r => r.CreateMessage(newMessage, chat.ChatId));

        // Act
        _chatService.CreateMessage(newMessage, chat.ChatId);

        // Assert
       
        _mockRepository.Verify(r => r.CreateMessage(newMessage, chat.ChatId), Times.Once);
    }
    
}
