using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Services;
using Moq;
using Npgsql;

namespace KamerConnect.UnitTests;

public class UpdateAccountTests
{
    private PersonService _mockPersonService;
    private Mock<IAuthenticationRepository> _mockRepository;
    private Mock<IPersonRepository> _mockPersonRepository;
    private Mock<NpgsqlConnection> _mockConnection;
    private Mock<NpgsqlCommand> _mockCommand;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IAuthenticationRepository>();
        _mockPersonRepository = new Mock<IPersonRepository>();
        _mockPersonService = new PersonService(_mockPersonRepository.Object);

        _mockConnection = new Mock<NpgsqlConnection>("mockConnectionString");
        _mockCommand = new Mock<NpgsqlCommand>();
    }

    [Test]
    public void UpdatePerson_ValidPerson_ExecutesUpdateQuery()
    {
        // Arrange
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            FirstName = "John",
            MiddleName = null, // Null to test default handling
            Surname = "Doe",
            PhoneNumber = null, // Null to test default handling
            BirthDate = new DateTime(1990, 1, 1),
            Gender = Gender.Male,
            Role = UserRole.User,
            ProfilePicturePath = null // Null to test default handling
        };

        // Mocking NpgsqlCommand.ExecuteNonQuery
        _mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Verifiable();

        // Mocking NpgsqlConnection behavior
        _mockConnection.Setup(conn => conn.Open());
        _mockConnection
            .Setup(conn => conn.CreateCommand())
            .Returns(_mockCommand.Object);

        // Act
        var personRepository = new PersonRepository(_mockConnection.Object);
        personRepository.UpdatePerson(person);

        // Assert
        _mockConnection.Verify(conn => conn.Open(), Times.Once);
        _mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@Id", person.Id), Times.Once);
        _mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@Email", person.Email), Times.Once);
        _mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@FirstName", person.FirstName), Times.Once);
        _mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@MiddleName", string.Empty), Times.Once);
        _mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@Surname", person.Surname), Times.Once);
        _mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@PhoneNumber", string.Empty), Times.Once);
        _mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@BirthDate", person.BirthDate), Times.Once);
        _mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@Gender", person.Gender.ToString()), Times.Once);
        _mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@Role", person.Role.ToString()), Times.Once);
        _mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@ProfilePicturePath", string.Empty), Times.Once);
        _mockCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
    }

    [Test]
    public void UpdatePerson_NullPerson_ThrowsArgumentNullException()
    {
        // Arrange
        Person nullPerson = null;

        var personRepository = new PersonRepository(_mockConnection.Object);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => personRepository.UpdatePerson(nullPerson));
    }

    [Test]
    public void UpdatePerson_InvalidConnection_ThrowsException()
    {
        // Arrange
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            FirstName = "John",
            Surname = "Doe"
        };

        _mockConnection.Setup(conn => conn.Open()).Throws(new InvalidOperationException("Cannot open connection"));

        var personRepository = new PersonRepository(_mockConnection.Object);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => personRepository.UpdatePerson(person));
    }
}