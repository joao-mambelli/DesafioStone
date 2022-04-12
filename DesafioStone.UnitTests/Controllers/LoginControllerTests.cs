using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DesafioStone.Controllers;
using Microsoft.AspNetCore.Mvc;
using DesafioStone.Interfaces.Services;
using DesafioStone.Utils.Common;
using System.Net;
using System.Net.Http;
using DesafioStone.DataContracts;
using DesafioStone.Models;

namespace DesafioStone.UnitTests.Controllers
{
    [TestClass]
    public class LoginControllerTests
    {
        [TestMethod]
        public void CanAuthorizeUser_ValidUser_Returns200()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserService>().Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<User>()).Verifiable();
            mock.Mock<ITokenService>().Setup(x => x.GenerateToken(It.IsAny<User>())).Returns(It.IsAny<string>()).Verifiable();
            mock.Mock<ITokenService>().Setup(x => x.GenerateAndSaveRefreshToken(It.IsAny<long?>())).Returns(It.IsAny<string>()).Verifiable();

            var controller = mock.Create<LoginController>();

            var result = controller.Login(new UserAuthorizeRequest());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 200);
        }

        [TestMethod]
        public void CanAuthorizeUser_InalidUser_Returns404()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserService>().Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Throws(NotFound()).Verifiable();

            var controller = mock.Create<LoginController>();

            var result = controller.Login(new UserAuthorizeRequest());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 404);
        }

        #region Helpers

        private static HttpRequestException NotFound()
        {
            return Helpers.BuildHttpException(HttpStatusCode.NotFound);
        }

        #endregion
    }
}
