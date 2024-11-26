using System;
using NUnit.Framework;
using KamerConnect.Utils;

namespace KamerConnect.UnitTests
{
    [TestFixture]
    public class FileUtilsTest
    {
        [Test]
        public void GetContentType_PngExtension_ReturnsImagePng()
        {
            // Arrange
            var fileName = "example.png";

            // Act
            var result = FileUtils.GetContentType(fileName);

            // Assert
            Assert.That(result, Is.EqualTo("image/png"));
        }

        [Test]
        public void GetContentType_JpgExtension_ReturnsImageJpeg()
        {
            // Arrange
            var fileName = "example.jpg";

            // Act
            var result = FileUtils.GetContentType(fileName);

            // Assert
            Assert.That(result, Is.EqualTo("image/jpeg"));
        }

        [Test]
        public void GetContentType_JpegExtension_ReturnsImageJpeg()
        {
            // Arrange
            var fileName = "example.jpeg";

            // Act
            var result = FileUtils.GetContentType(fileName);

            // Assert
            Assert.That(result, Is.EqualTo("image/jpeg"));
        }

        [Test]
        public void GetContentType_GifExtension_ReturnsImageGif()
        {
            // Arrange
            var fileName = "example.gif";

            // Act
            var result = FileUtils.GetContentType(fileName);

            // Assert
            Assert.That(result, Is.EqualTo("image/gif"));
        }

        [Test]
        public void GetContentType_BmpExtension_ReturnsImageBmp()
        {
            // Arrange
            var fileName = "example.bmp";

            // Act
            var result = FileUtils.GetContentType(fileName);

            // Assert
            Assert.That(result, Is.EqualTo("image/bmp"));
        }

        [Test]
        public void GetContentType_TiffExtension_ReturnsImageTiff()
        {
            // Arrange
            var fileName = "example.tiff";

            // Act
            var result = FileUtils.GetContentType(fileName);

            // Assert
            Assert.That(result, Is.EqualTo("image/tiff"));
        }

        [Test]
        public void GetContentType_UnknownExtension_ReturnsApplicationOctetStream()
        {
            // Arrange
            var fileName = "example.unknown";

            // Act
            var result = FileUtils.GetContentType(fileName);

            // Assert
            Assert.That(result, Is.EqualTo("application/octet-stream"));
        }

        [Test]
        public void GetContentType_NoExtension_ReturnsApplicationOctetStream()
        {
            // Arrange
            var fileName = "example";

            // Act
            var result = FileUtils.GetContentType(fileName);

            // Assert
            Assert.That(result, Is.EqualTo("application/octet-stream"));
        }

        [Test]
        public void GetContentType_EmptyFileName_ReturnsApplicationOctetStream()
        {
            // Arrange
            var fileName = "";

            // Act
            var result = FileUtils.GetContentType(fileName);

            // Assert
            Assert.That(result, Is.EqualTo("application/octet-stream"));
        }

        [Test]
        public void GetContentType_NullFileName_ReturnsApplicationOctetStream()
        {
            // Arrange
            string fileName = null;

            // Act
            var result = FileUtils.GetContentType(fileName);

            // Assert
            Assert.That(result, Is.EqualTo("application/octet-stream"));
        }
    }
}
