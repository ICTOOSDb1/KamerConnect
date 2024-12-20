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

}
