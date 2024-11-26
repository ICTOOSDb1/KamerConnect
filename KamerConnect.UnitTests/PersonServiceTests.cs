using KamerConnect.Models;
using Moq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using NUnit;
using KamerConnect.Repositories;

namespace KamerConnect.UnitTests
{
    public class PersonServiceTests
    {
        //    public void UpdatePerson_ValidPerson_ExecutesNonQueryWithCorrectParameters()
        //    {
        //        // Arrange
        //        var connectionString = "Host=localhost;Database=testdb;Username=user;Password=password"; // Replace with your test connection string
        //        var repository = new PersonRepository(connectionString);

        //        var person = new Person
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            Email = "test@example.com",
        //            FirstName = "Test",
        //            MiddleName = null,
        //            Surname = "User",
        //            PhoneNumber = null,
        //            BirthDate = new DateTime(2000, 1, 1),
        //            Gender = Gender.Male,
        //            Role = Role.Offering,
        //            ProfilePicturePath = null
        //        };

        //        var mockConnection = new Mock<NpgsqlConnection>(connectionString) { CallBase = true };
        //        var mockCommand = new Mock<NpgsqlCommand>();

        //        mockConnection
        //            .Setup(conn => conn.Open())
        //            .Verifiable();

        //        mockCommand
        //            .SetupSet(cmd => cmd.CommandText = It.IsAny<string>())
        //            .Verifiable();

        //        mockCommand
        //            .Setup(cmd => cmd.ExecuteNonQuery())
        //            .Verifiable();

        //        mockConnection
        //            .Protected()
        //            .Setup<NpgsqlCommand>("CreateDbCommand")
        //            .Returns(mockCommand.Object);

        //        // Act
        //        repository.UpdatePerson(person);

        //        // Assert
        //        mockConnection.Verify(conn => conn.Open(), Times.Once);
        //        mockCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);

        //        mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@Id", Guid.Parse(person.Id)), Times.Once);
        //        mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@Email", person.Email), Times.Once);
        //        mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@FirstName", person.FirstName), Times.Once);
        //        mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@MiddleName", string.Empty), Times.Once);
        //        mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@Surname", person.Surname), Times.Once);
        //        mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@PhoneNumber", string.Empty), Times.Once);
        //        mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@BirthDate", person.BirthDate), Times.Once);
        //        mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@Gender", person.Gender.ToString()), Times.Once);
        //        mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@Role", person.Role.ToString()), Times.Once);
        //        mockCommand.Verify(cmd => cmd.Parameters.AddWithValue("@ProfilePicturePath", string.Empty), Times.Once);
        //    }
        //}

    }
}

