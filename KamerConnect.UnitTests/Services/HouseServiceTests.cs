using System;
using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Services;
using Moq;
using NUnit.Framework;

namespace KamerConnect.UnitTests
{
    [TestFixture]
    public class HouseServiceTests
    {
        private HouseService _houseService;
        private Mock<IHouseRepository> _mockRepository;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IHouseRepository>();
            _houseService = new HouseService(_mockRepository.Object);
        }

        [Test]
        public void Create_WhenCalled_ReturnsNewHouseId()
        {
            // Arrange
            var newHouse = TestModels.HouseModel;
            var newHouseId = Guid.NewGuid();
            _mockRepository.Setup(r => r.Create(newHouse, Guid.NewGuid())).Returns(newHouseId);

            // Act
            var result = _houseService.Create(newHouse, Guid.NewGuid());

            // Assert
            Assert.That(newHouseId, Is.EqualTo(result));
            _mockRepository.Verify(r => r.Create(newHouse, Guid.NewGuid()), Times.Once);
        }

        [Test]
        public void Update_WhenCalled_InvokesRepositoryUpdate()
        {
            // Arrange
            var houseToUpdate = TestModels.HouseModel;

            // Act
            _houseService.Update(houseToUpdate);

            // Assert
            _mockRepository.Verify(r => r.Update(houseToUpdate), Times.Once);
        }
    }
}
