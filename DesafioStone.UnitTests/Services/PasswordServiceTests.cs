using Autofac.Extras.Moq;
using DesafioStone.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesafioStone.Interfaces.Services;
using Moq;

namespace DesafioStone.UnitTests.Services
{
    [TestClass]
    public class PasswordServiceTests
    {
        [TestMethod]
        public void CanValidate_RightPassword_ReturnsTrue()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IHashService>().Setup(x => x.CompareHash(It.IsAny<string>(), It.IsAny<string>())).Returns(true).Verifiable();

            var service = mock.Create<PasswordService>();

            var result = service.IsValid(It.IsAny<string>(), It.IsAny<string>());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanValidate_WrongPassword_ReturnsFalse()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IHashService>().Setup(x => x.CompareHash(It.IsAny<string>(), It.IsAny<string>())).Returns(false).Verifiable();

            var service = mock.Create<PasswordService>();

            var result = service.IsValid(It.IsAny<string>(), It.IsAny<string>());

            Assert.IsTrue(!result);
        }
    }
}