using System;
using NUnit.Framework;
using KamerConnect.Utils;

namespace KamerConnect.UnitTests
{
    [TestFixture]
    public class EnumUtilsTest
    {
        private enum SampleEnum
        {
            FirstValue,
            SecondValue,
            ThirdValue
        }

        [Test]
        public void Validate_ValidValue_ReturnsEnum()
        {
            // Arrange
            var input = "FirstValue";

            // Act
            var result = EnumUtils.Validate<SampleEnum>(input);

            // Assert
            Assert.That(result, Is.EqualTo(SampleEnum.FirstValue));
        }

        [Test]
        public void Validate_InvalidValue_ThrowsArgumentException()
        {
            // Arrange
            var input = "InvalidValue";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => EnumUtils.Validate<SampleEnum>(input));
            Assert.That(ex.Message, Is.EqualTo("Invalid value 'InvalidValue' for enum SampleEnum"));
        }

        [Test]
        public void Validate_EmptyString_ThrowsArgumentException()
        {
            // Arrange
            var input = "";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => EnumUtils.Validate<SampleEnum>(input));
            Assert.That(ex.Message, Is.EqualTo("Invalid value '' for enum SampleEnum"));
        }
    }
}
