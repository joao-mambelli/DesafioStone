using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesafioStone.Repositories;
using DesafioStone.Interfaces.Factories;
using System.Data;
using DesafioStone.Models;
using DesafioStone.DataContracts;
using Moq;

namespace DesafioStone.UnitTests.Repositories
{
    [TestClass]
    public class InvoiceRepositoryTests
    {
        [TestMethod]
        public void CanGetInvoices_ReturnsInvoices()
        {
            using var mock = AutoMock.GetLoose();

            var commandMock = mock.Mock<IDbCommand>();

            var connectionMock = mock.Mock<IDbConnection>();
            connectionMock.Setup(x => x.CreateCommand()).Returns(commandMock.Object);

            var connectionFactoryMock = mock.Mock<IDbConnectionFactory>();
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

            var repository = mock.Create<InvoiceRepository>();

            var result = repository.GetInvoices(new InvoiceQuery());

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void CanGetInvoiceById_ReturnsInvoice()
        {
            using var mock = AutoMock.GetLoose();

            var commandMock = mock.Mock<IDbCommand>();

            var connectionMock = mock.Mock<IDbConnection>();
            connectionMock.Setup(x => x.CreateCommand()).Returns(commandMock.Object);

            var connectionFactoryMock = mock.Mock<IDbConnectionFactory>();
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

            var repository = mock.Create<InvoiceRepository>();

            var result = repository.GetInvoiceById(It.IsAny<long>());

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void CanGetCount_ReturnsLong()
        {
            using var mock = AutoMock.GetLoose();

            var commandMock = mock.Mock<IDbCommand>();

            var connectionMock = mock.Mock<IDbConnection>();
            connectionMock.Setup(x => x.CreateCommand()).Returns(commandMock.Object);

            var connectionFactoryMock = mock.Mock<IDbConnectionFactory>();
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

            var repository = mock.Create<InvoiceRepository>();

            var result = repository.GetInvoiceCount();

            Assert.AreEqual(0, result);
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

            var repository = mock.Create<InvoiceRepository>();

            var result = repository.InsertInvoice(new Invoice());

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

            var repository = mock.Create<InvoiceRepository>();

            repository.UpdateInvoice(new Invoice());

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

            var repository = mock.Create<InvoiceRepository>();

            repository.DeleteInvoice(It.IsAny<long>());

            commandMock.Verify();
        }
    }
}
