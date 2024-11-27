using System;
using KamerConnect.Models;
using KamerConnect.Utils;
using NUnit.Framework;

namespace KamerConnect.UnitTests
{
    [TestFixture]
    public class ValidationUtilsTest
    {
        [Test]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";

            // Act
            var result = ValidationUtils.IsValidEmail(email);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidEmail_InvalidEmail_ReturnsFalse()
        {
            // Arrange
            var email = "test@.com";

            // Act
            var result = ValidationUtils.IsValidEmail(email);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsInteger_ValidIntegerString_ReturnsTrue()
        {
            // Arrange
            var text = "123";

            // Act
            var result = ValidationUtils.IsInteger(text);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsInteger_InvalidIntegerString_ReturnsFalse()
        {
            // Arrange
            var text = "12.34";

            // Act
            var result = ValidationUtils.IsInteger(text);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsDouble_ValidDoubleString_ReturnsTrue()
        {
            // Arrange
            var text = "12.34";

            // Act
            var result = ValidationUtils.IsDouble(text);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsDouble_InvalidDoubleString_ReturnsFalse()
        {
            // Arrange
            var text = "abcd";

            // Act
            var result = ValidationUtils.IsDouble(text);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValidPostalCode_ValidPostalCode_ReturnsTrue()
        {
            // Arrange
            var postalCode = "1234 AB";

            // Act
            var result = ValidationUtils.IsValidPostalCode(postalCode);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidPostalCode_InvalidPostalCode_ReturnsFalse()
        {
            // Arrange
            var postalCode = "12345";

            // Act
            var result = ValidationUtils.IsValidPostalCode(postalCode);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValidPhoneNumber_ValidPhoneNumber_ReturnsTrue()
        {
            // Arrange
            var phoneNumber = "+31612345678";

            // Act
            var result = ValidationUtils.IsValidPhoneNumber(phoneNumber);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidPhoneNumber_InvalidPhoneNumber_ReturnsFalse()
        {
            // Arrange
            var phoneNumber = "12345";

            // Act
            var result = ValidationUtils.IsValidPhoneNumber(phoneNumber);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
