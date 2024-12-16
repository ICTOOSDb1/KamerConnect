using System;
using System.Threading.Tasks;
using KamerConnect;
using KamerConnect.EnvironmentVariables;
using KamerConnect.Exceptions;
using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Services;
using Moq;
using NUnit.Framework;

namespace KamerConnect.UnitTests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private PersonService _mockPersonService;
        private Mock<IAuthenticationRepository> _mockRepository;
        private Mock<IPersonRepository> _mockPersonRepository;
        private AuthenticationService _authenticationService;

        [SetUp]
        public void SetUp()
        {
            EnvVariables.Load();

            // Arrange: Initialize mocks and service
            _mockRepository = new Mock<IAuthenticationRepository>();
            _mockPersonRepository = new Mock<IPersonRepository>();
            _mockPersonService = new PersonService(_mockPersonRepository.Object);
            _authenticationService = new AuthenticationService(_mockPersonService, _mockRepository.Object);
        }

        [Test]
        public void Authenticate_InvalidCredentials_ThrowsInvalidCredentialsException()
        {
            // Arrange
            string email = "test@example.com";
            string passwordAttempt = "wrongpassword";

            _mockPersonRepository.Setup(service => service.GetPersonByEmail(email)).Throws(new InvalidCredentialsException());

            // Act & Assert
            Assert.ThrowsAsync<InvalidCredentialsException>(async () => await _authenticationService.Authenticate(email, passwordAttempt));
        }

        // [Test]
        public void Register_InvalidEmail_ThrowsInvalidOperationException()
        {
            // Arrange
            var person = new Person("invalid-email", "John", "Middle", "Doe", "123456789", DateTime.Now, Gender.Male, Role.Seeking, "path/to/profile.jpg", Guid.NewGuid());
            string password = "validpassword";

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _authenticationService.Register(person, password));
        }
    }
}
