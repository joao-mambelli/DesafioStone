using Autofac.Extras.Moq;
using DesafioStone.Interfaces.Repositories;
using DesafioStone.Models;
using DesafioStone.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using DesafioStone.DataContracts;
using Moq;
using DesafioStone.Interfaces.Services;

namespace DesafioStone.UnitTests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public void CanVerifyPassword_UserExistsRightPassword_ReturnsUser()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserRepository>().Setup(x => x.GetUserByUsername(It.IsAny<string>(), It.IsAny<bool>())).Returns(new User()).Verifiable();
            mock.Mock<IPasswordService>().Setup(x => x.IsValid(It.IsAny<string>(), It.IsAny<string>())).Returns(true).Verifiable();

            var service = mock.Create<UserService>();

            var result = service.VerifyPassword(It.IsAny<string>(), It.IsAny<string>());

            Assert.IsTrue(result != null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void CanVerifyPassword_UserExistsWrongPassword_ReturnsUser()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserRepository>().Setup(x => x.GetUserByUsername(It.IsAny<string>(), It.IsAny<bool>())).Returns(new User()).Verifiable();
            mock.Mock<IPasswordService>().Setup(x => x.IsValid(It.IsAny<string>(), It.IsAny<string>())).Returns(false).Verifiable();

            var service = mock.Create<UserService>();

            service.VerifyPassword(It.IsAny<string>(), It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void CanVerifyPassword_UserDoesNotExists_ThrowsException()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserRepository>().Setup(x => x.GetUserByUsername(It.IsAny<string>(), It.IsAny<bool>())).Returns(It.IsAny<User>()).Verifiable();

            var service = mock.Create<UserService>();

            service.VerifyPassword(It.IsAny<string>(), It.IsAny<string>());
        }

        [TestMethod]
        public void CanGetById_UserExists_ReturnsUser()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserRepository>().Setup(x => x.GetUserById(It.IsAny<long>(), It.IsAny<bool>())).Returns(new User()).Verifiable();

            var service = mock.Create<UserService>();

            var result = service.GetUserById(It.IsAny<long>());

            Assert.IsTrue(result != null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void CanGetById_UserDoesNotExist_ThrowsException()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserRepository>().Setup(x => x.GetUserById(It.IsAny<long>(), It.IsAny<bool>())).Returns(It.IsAny<User>()).Verifiable();

            var service = mock.Create<UserService>();

            service.GetUserById(It.IsAny<long>());
        }

        [TestMethod]
        public void CanGetByUsername_UserExists_ReturnsUser()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserRepository>().Setup(x => x.GetUserByUsername(It.IsAny<string>(), It.IsAny<bool>())).Returns(new User()).Verifiable();

            var service = mock.Create<UserService>();

            var result = service.GetUserByUsername(It.IsAny<string>());

            Assert.IsTrue(result != null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void CanGetByUsername_UserDoesNotExist_ThrowsException()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserRepository>().Setup(x => x.GetUserByUsername(It.IsAny<string>(), It.IsAny<bool>())).Returns(It.IsAny<User>()).Verifiable();

            var service = mock.Create<UserService>();

            service.GetUserByUsername(It.IsAny<string>());
        }

        [TestMethod]
        public void CanCreateUser_UsernameDoesNotExist_ReturnsUser()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserRepository>().Setup(x => x.GetUserByUsername(It.IsAny<string>(), It.IsAny<bool>())).Returns(It.IsAny<User>()).Verifiable();
            mock.Mock<IUserRepository>().Setup(x => x.InsertUser(It.IsAny<User>())).Returns(It.IsAny<long>()).Verifiable();
            mock.Mock<IUserRepository>().Setup(x => x.GetUserById(It.IsAny<long>(), It.IsAny<bool>())).Returns(new User()).Verifiable();

            var service = mock.Create<UserService>();

            var result = service.CreateUser(new UserCreateRequest());

            Assert.IsTrue(result != null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void CanCreateUser_UsernameAlreadyExists_ReturnsUser()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserRepository>().Setup(x => x.GetUserByUsername(It.IsAny<string>(), It.IsAny<bool>())).Returns(new User()).Verifiable();

            var service = mock.Create<UserService>();

            service.CreateUser(new UserCreateRequest());
        }

        [TestMethod]
        public void CanUpdateUserPassword_UserExists_ReturnsUser()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserRepository>().Setup(x => x.GetUserById(It.IsAny<long>(), It.IsAny<bool>())).Returns(new User()).Verifiable();
            mock.Mock<IHashService>().Setup(x => x.ComputeHash(It.IsAny<string>())).Returns(It.IsAny<string>()).Verifiable();
            mock.Mock<IUserRepository>().Setup(x => x.UpdateUser(It.IsAny<User>())).Verifiable();

            var service = mock.Create<UserService>();

            var result = service.UpdateUserPassword(new UserUpdatePasswordRequest(), 1);

            Assert.IsTrue(result != null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void CanUpdateUserPassword_UserDoesNotExist_ThrowsException()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserRepository>().Setup(x => x.GetUserById(It.IsAny<long>(), It.IsAny<bool>())).Returns(It.IsAny<User>()).Verifiable();

            var service = mock.Create<UserService>();

            service.UpdateUserPassword(It.IsAny<UserUpdatePasswordRequest>(), 1);
        }

        [TestMethod]
        public void CanDelete_UserExists_ReturnsVoid()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserRepository>().Setup(x => x.GetUserById(It.IsAny<long>(), It.IsAny<bool>())).Returns(new User()).Verifiable();
            mock.Mock<IUserRepository>().Setup(x => x.DeleteUser(It.IsAny<long>())).Verifiable();

            var service = mock.Create<UserService>();

            service.DeleteUser(It.IsAny<long>());
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void CanDelete_UserDoesNotExist_ThrowsException()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IUserRepository>().Setup(x => x.GetUserById(It.IsAny<long>(), It.IsAny<bool>())).Returns(It.IsAny<User>()).Verifiable();

            var service = mock.Create<UserService>();

            service.DeleteUser(It.IsAny<long>());
        }
    }
}