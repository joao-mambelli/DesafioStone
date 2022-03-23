using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesafioStone.Repositories;
using DesafioStone.Interfaces.Factories;
using System.Data;
using DesafioStone.Models;
using Moq;

namespace DesafioStone.UnitTests.Repositories
{
    [TestClass]
    public class UserRepositoryTests
    {
        [TestMethod]
        public void CanGetAllUsers_ReturnsUsers()
        {
            using var mock = AutoMock.GetLoose();

            var commandMock = mock.Mock<IDbCommand>();

            var connectionMock = mock.Mock<IDbConnection>();
            connectionMock.Setup(x => x.CreateCommand()).Returns(commandMock.Object);

            var connectionFactoryMock = mock.Mock<IDbConnectionFactory>();
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

            var repository = mock.Create<UserRepository>();

            var result = repository.GetAllUsers();

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void CanGetUserById_ReturnsUser()
        {
            using var mock = AutoMock.GetLoose();

            var commandMock = mock.Mock<IDbCommand>();

            var connectionMock = mock.Mock<IDbConnection>();
            connectionMock.Setup(x => x.CreateCommand()).Returns(commandMock.Object);

            var connectionFactoryMock = mock.Mock<IDbConnectionFactory>();
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

            var repository = mock.Create<UserRepository>();

            var result = repository.GetUserById(It.IsAny<long>());

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void CanGetUserByUsername_ReturnsUser()
        {
            using var mock = AutoMock.GetLoose();

            var commandMock = mock.Mock<IDbCommand>();

            var connectionMock = mock.Mock<IDbConnection>();
            connectionMock.Setup(x => x.CreateCommand()).Returns(commandMock.Object);

            var connectionFactoryMock = mock.Mock<IDbConnectionFactory>();
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

            var repository = mock.Create<UserRepository>();

            var result = repository.GetUserByUsername(It.IsAny<string>());

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void CanInsert_ReturnsLong()
        {
            using var mock = AutoMock.GetLoose();

            var commandMock = mock.Mock<IDbCommand>();
            commandMock.Setup(x => x.ExecuteNonQuery()).Verifiable();

            var connectionMock = mock.Mock<IDbConnection>();
            connectionMock.Setup(x => x.CreateCommand()).Returns(commandMock.Object);

            var connectionFactoryMock = mock.Mock<IDbConnectionFactory>();
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

            var repository = mock.Create<UserRepository>();

            var result = repository.InsertUser(new User());

            commandMock.Verify();
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CanUpdate_ReturnsVoid()
        {
            using var mock = AutoMock.GetLoose();

            var commandMock = mock.Mock<IDbCommand>();
            commandMock.Setup(x => x.ExecuteNonQuery()).Verifiable();

            var connectionMock = mock.Mock<IDbConnection>();
            connectionMock.Setup(x => x.CreateCommand()).Returns(commandMock.Object);

            var connectionFactoryMock = mock.Mock<IDbConnectionFactory>();
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

            var repository = mock.Create<UserRepository>();

            repository.UpdateUser(new User());

            commandMock.Verify();
        }

        [TestMethod]
        public void CanDelete_ReturnsVoid()
        {
            using var mock = AutoMock.GetLoose();

            var commandMock = mock.Mock<IDbCommand>();
            commandMock.Setup(x => x.ExecuteNonQuery()).Verifiable();

            var connectionMock = mock.Mock<IDbConnection>();
            connectionMock.Setup(x => x.CreateCommand()).Returns(commandMock.Object);

            var connectionFactoryMock = mock.Mock<IDbConnectionFactory>();
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

            var repository = mock.Create<UserRepository>();

            repository.DeleteUser(It.IsAny<long>());

            commandMock.Verify();
        }
    }
}
