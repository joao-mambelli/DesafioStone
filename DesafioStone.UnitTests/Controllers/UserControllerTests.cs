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
        public void CanAuthorizeUser_ValidUser_Returns200()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserService>().Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<User>()).Verifiable();
            mock.Mock<ITokenService>().Setup(x => x.GenerateToken(It.IsAny<User>())).Returns(It.IsAny<string>()).Verifiable();
            mock.Mock<ITokenService>().Setup(x => x.GenerateAndSaveRefreshToken(It.IsAny<long?>())).Returns(It.IsAny<string>()).Verifiable();

            var controller = mock.Create<UserController>();

            var result = controller.AuthorizeUser(new UserAuthorizeRequest());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 200);
        }

        [TestMethod]
        public void CanAuthorizeUser_InalidUser_Returns404()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserService>().Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Throws(NotFound()).Verifiable();

            var controller = mock.Create<UserController>();

            var result = controller.AuthorizeUser(new UserAuthorizeRequest());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 404);
        }

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
        public void CanUpdateUserPassword_IsSameUserAndUserExists_Returns200()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserService>().Setup(x => x.UpdateUserPassword(It.IsAny<UserUpdatePasswordRequest>(), It.IsAny<long>())).Returns(It.IsAny<User>()).Verifiable();
            
            var controller = mock.Create<UserController>();

            var id = 0;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { 
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                            {
                                new Claim("Id", id.ToString())
                            }
                        )
                    )
                }
            };

            var result = controller.UpdateUserPassword(It.IsAny<UserUpdatePasswordRequest>(), id);

            Assert.IsTrue(((ObjectResult)result).StatusCode == 200);
        }

        [TestMethod]
        public void CanUpdateUserPassword_IsNotSameUser_Returns401()
        {
            using var mock = AutoMock.GetLoose();

            var controller = mock.Create<UserController>();

            var id = 0;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                            {
                                new Claim("Id", id.ToString())
                            }
                        )
                    )
                }
            };

            var result = controller.UpdateUserPassword(It.IsAny<UserUpdatePasswordRequest>(), id + 1);

            Assert.IsTrue(((ObjectResult)result).StatusCode == 401);
        }

        [TestMethod]
        public void CanUpdateUserPassword_UserDoesNotExist_Returns404()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserService>().Setup(x => x.UpdateUserPassword(It.IsAny<UserUpdatePasswordRequest>(), It.IsAny<long>())).Throws(NotFound()).Verifiable();

            var controller = mock.Create<UserController>();

            var id = 0;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                            {
                                new Claim("Id", id.ToString())
                            }
                        )
                    )
                }
            };

            var result = controller.UpdateUserPassword(It.IsAny<UserUpdatePasswordRequest>(), id);

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
