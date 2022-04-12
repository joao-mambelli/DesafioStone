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
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DesafioStone.UnitTests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public void CanGetUserById_UserExists_Returns200()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserService>().Setup(x => x.GetUserById(It.IsAny<long>())).Returns(It.IsAny<User>()).Verifiable();

            var controller = mock.Create<UserController>();

            var result = controller.GetUserById(It.IsAny<long>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 200);
        }

        [TestMethod]
        public void CanGetUserById_UserDoesNotExist_Returns404()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserService>().Setup(x => x.GetUserById(It.IsAny<long>())).Throws(NotFound()).Verifiable();

            var controller = mock.Create<UserController>();

            var result = controller.GetUserById(It.IsAny<long>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 404);
        }

        [TestMethod]
        public void CanCreateUser_UsernameDoesNotExist_Returns201()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserService>().Setup(x => x.CreateUser(It.IsAny<UserCreateRequest>())).Returns(new User()).Verifiable();

            var controller = mock.Create<UserController>();

            var result = controller.CreateUser(It.IsAny<UserCreateRequest>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 201);
        }

        [TestMethod]
        public void CanCreateUser_UsernameAlreadyExists_Returns409()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserService>().Setup(x => x.CreateUser(It.IsAny<UserCreateRequest>())).Throws(Conflict()).Verifiable();

            var controller = mock.Create<UserController>();

            var result = controller.CreateUser(It.IsAny<UserCreateRequest>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 409);
        }

        [TestMethod]
        public void CanUpdateUserPassword_UserExists_Returns200()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserService>().Setup(x => x.UpdateUserPassword(It.IsAny<UserUpdatePasswordRequest>(), It.IsAny<long>())).Returns(It.IsAny<User>()).Verifiable();
            
            var controller = mock.Create<UserController>();

            var id = 0;
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["UserId"] = id.ToString();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var result = controller.UpdateUserPassword(It.IsAny<UserUpdatePasswordRequest>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 200);
        }

        [TestMethod]
        public void CanUpdateUserPassword_UserDoesNotExist_Returns404()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserService>().Setup(x => x.UpdateUserPassword(It.IsAny<UserUpdatePasswordRequest>(), It.IsAny<long>())).Throws(NotFound()).Verifiable();

            var controller = mock.Create<UserController>();

            var id = 0;
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["UserId"] = id.ToString();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var result = controller.UpdateUserPassword(It.IsAny<UserUpdatePasswordRequest>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 404);
        }

        [TestMethod]
        public void CanDeleteUser_UserExists_Returns200()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserService>().Setup(x => x.DeleteUser(It.IsAny<long>())).Verifiable();

            var controller = mock.Create<UserController>();

            var result = controller.DeleteUser(It.IsAny<long>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 200);
        }

        [TestMethod]
        public void CanDeleteUser_UserDoesNotExist_Returns404()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserService>().Setup(x => x.DeleteUser(It.IsAny<long>())).Throws(NotFound()).Verifiable();

            var controller = mock.Create<UserController>();

            var result = controller.DeleteUser(It.IsAny<long>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 404);
        }

        #region Helpers

        private static HttpRequestException NotFound()
        {
            return Helpers.BuildHttpException(HttpStatusCode.NotFound);
        }

        private static HttpRequestException Conflict()
        {
            return Helpers.BuildHttpException(HttpStatusCode.Conflict);
        }

        #endregion
    }
}
