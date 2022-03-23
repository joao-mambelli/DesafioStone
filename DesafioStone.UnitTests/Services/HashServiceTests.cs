using Autofac.Extras.Moq;
using DesafioStone.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesafioStone.UnitTests.Services
{
    [TestClass]
    public class HashServiceTests
    {
        [TestMethod]
        public void CanCompareHash_RightPassword_ReturnsTrue()
        {
            using var mock = AutoMock.GetLoose();

            var service = mock.Create<HashService>();

            var hash = service.ComputeHash("test");

            var result = service.CompareHash("test", hash);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanCompareHash_WrongPassword_ReturnsFalse()
        {
            using var mock = AutoMock.GetLoose();

            var service = mock.Create<HashService>();

            var hash = service.ComputeHash("test");

            var result = service.CompareHash("wrongPassword", hash);

            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void CanCompareHash_InvalidHash_ReturnsFalse()
        {
            using var mock = AutoMock.GetLoose();

            var service = mock.Create<HashService>();

            var result = service.CompareHash("test", "");

            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void CanComputeHash_ReturnsHashedString()
        {
            using var mock = AutoMock.GetLoose();

            var service = mock.Create<HashService>();

            var result = service.ComputeHash("test");

            Assert.IsTrue(service.CompareHash("test", result));
        }
    }
}