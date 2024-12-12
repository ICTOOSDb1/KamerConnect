using System.Data;
using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Services;
using Moq;
using MatchModel = KamerConnect.Models.Match;

namespace KamerConnect.UnitTests;

public class MatchTests
{
    private Mock<IMatchRepository> _mockRepository;
    private MatchService _matchService;
    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IMatchRepository>();
        _matchService = new MatchService(_mockRepository.Object);
    }




    [Test]
    public void GetMatchesById_WhenCalled_ReturnsMatchingResults()
    {
        // Arrange
        var mockId = Guid.NewGuid();
        var expectedMatches = new List<MatchModel>
        {
            new (Guid.NewGuid(), mockId, Guid.NewGuid(), Status.Pending, "test"),
            new (Guid.NewGuid(), mockId, Guid.NewGuid(), Status.Pending, "test"),

        };
        _mockRepository.Setup(repo => repo.GetPendingMatchesById(mockId))
            .Returns(expectedMatches);
        // Act
        var result = _matchService.GetMatchesById(mockId);
        // Assert
        Assert.That(result, Is.EquivalentTo(expectedMatches));
        _mockRepository.Verify(repo => repo.GetPendingMatchesById(mockId), Times.Once);
    }


    [Test]
    public void UpdateMatch_WhenCalled_UpdatesMatchStatus()
    {
        // Arrange
        var mockMatch = new MatchModel(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Status.Pending, "test");
        var newStatus = Status.Pending;

        // Act
        _matchService.UpdateStatusMatch(mockMatch, newStatus);

        // Assert
        _mockRepository.Verify(repo => repo.UpdateStatusMatch(mockMatch, newStatus), Times.Once);
    }


}