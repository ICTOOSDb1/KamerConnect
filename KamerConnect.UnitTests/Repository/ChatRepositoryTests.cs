using KamerConnect.DataAccess.Postgres.Repositories;
using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Services;
using Moq;
using Npgsql;

namespace KamerConnect.UnitTests.Repository;

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
    public void GetChatMessages_ReturnsExpectedMessages()
    {
        var chatId = Guid.NewGuid();
        var expectedMessages = new List<ChatMessage>
        {
            new ChatMessage(Guid.NewGuid(), chatId, "Hello", DateTime.UtcNow),
            new ChatMessage(Guid.NewGuid(), chatId, "Hi", DateTime.UtcNow)
        };

        _mockRepository.Setup(repo => repo.GetChatMessages(chatId))
            .Returns(expectedMessages);

        var actualMessages = _chatService.GetChatMessages(chatId);

        Assert.That(actualMessages.Count, Is.EqualTo(expectedMessages.Count));
        Assert.That(actualMessages, Is.EquivalentTo(expectedMessages));
    }

    [Test]
    public void CreateMessage_CreatesSuccessfully()
    {
        var message = new ChatMessage(Guid.NewGuid(), Guid.NewGuid(), "Hello", DateTime.UtcNow);
        var chatId = Guid.NewGuid();

        _mockRepository.Setup(repo => repo.CreateMessage(message, chatId));

        Assert.That(() => _chatService.CreateMessage(message, chatId), Throws.Nothing);
    }

    [Test]
    public void CreateMessage_ThrowsExceptionOnError()
    {
        var message = new ChatMessage(Guid.NewGuid(), Guid.NewGuid(), "Hello", DateTime.UtcNow);
        var chatId = Guid.NewGuid();

        _mockRepository.Setup(repo => repo.CreateMessage(message, chatId))
            .Throws(new NpgsqlException("Database error"));

        Assert.That(() => _chatService.CreateMessage(message, chatId), Throws.TypeOf<NpgsqlException>());
    }
}
