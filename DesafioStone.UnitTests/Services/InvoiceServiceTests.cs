using Autofac.Extras.Moq;
using DesafioStone.Interfaces.Repositories;
using DesafioStone.Models;
using DesafioStone.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using DesafioStone.DataContracts;
using System.Collections.Generic;
using Moq;
using Microsoft.AspNetCore.JsonPatch;

namespace DesafioStone.UnitTests.Services
{
    [TestClass]
    public class InvoiceServiceTests
    {
        [TestMethod]
        public void CanGet_ReturnsListOfInvoices()
        {
            using var mock = AutoMock.GetLoose();
            var query = new InvoiceQuery();

            mock.Mock<IInvoiceRepository>().Setup(x => x.GetInvoices(It.IsAny<InvoiceQuery>(), It.IsAny<bool>())).Returns(new List<Invoice>()).Verifiable();

            var service = mock.Create<InvoiceService>();

            var result = service.GetInvoices(It.IsAny<InvoiceQuery>());

            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public void CanGetById_InvoiceExists_ReturnsInvoice()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceRepository>().Setup(x => x.GetInvoiceById(It.IsAny<long>(), It.IsAny<bool>())).Returns(new Invoice()).Verifiable();

            var service = mock.Create<InvoiceService>();

            var result = service.GetInvoiceById(It.IsAny<long>());

            Assert.IsTrue(result != null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void CanGetById_InvoiceDoesNotExist_ThrowsException()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceRepository>().Setup(x => x.GetInvoiceById(It.IsAny<long>(), It.IsAny<bool>())).Returns(It.IsAny<Invoice>()).Verifiable();

            var service = mock.Create<InvoiceService>();

            service.GetInvoiceById(It.IsAny<long>());
        }

        [TestMethod]
        public void CanGetNumberOfInvoices_ThereInvoices_ReturnsLong()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceRepository>().Setup(x => x.GetInvoiceCount(It.IsAny<bool>())).Returns(1).Verifiable();

            var service = mock.Create<InvoiceService>();

            var result = service.GetNumberOfInvoices();

            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        public void CanCreateInvoice_ReturnsInvoice()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceRepository>().Setup(x => x.InsertInvoice(It.IsAny<Invoice>())).Returns(It.IsAny<long>()).Verifiable();
            mock.Mock<IInvoiceRepository>().Setup(x => x.GetInvoiceById(It.IsAny<long>(), It.IsAny<bool>())).Returns(new Invoice()).Verifiable();

            var service = mock.Create<InvoiceService>();

            var result = service.CreateInvoice(new InvoiceCreateRequest());

            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public void CanUpdateInvoice_InvoiceExists_ReturnsInvoice()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceRepository>().Setup(x => x.GetInvoiceById(It.IsAny<long>(), It.IsAny<bool>())).Returns(new Invoice()).Verifiable();
            mock.Mock<IInvoiceRepository>().Setup(x => x.UpdateInvoice(It.IsAny<Invoice>())).Verifiable();

            var service = mock.Create<InvoiceService>();

            var result = service.UpdateInvoice(new InvoiceUpdateRequest(), It.IsAny<long>());

            Assert.IsTrue(result != null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void CanUpdateInvoice_InvoiceDoesNotExist_ThrowsException()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceRepository>().Setup(x => x.GetInvoiceById(It.IsAny<long>(), It.IsAny<bool>())).Returns(It.IsAny<Invoice>()).Verifiable();

            var service = mock.Create<InvoiceService>();

            service.UpdateInvoice(new InvoiceUpdateRequest(), It.IsAny<long>());
        }

        [TestMethod]
        public void CanPatchInvoice_InvoiceExists_ReturnsInvoice()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceRepository>().Setup(x => x.GetInvoiceById(It.IsAny<long>(), It.IsAny<bool>())).Returns(new Invoice()).Verifiable();
            mock.Mock<IInvoiceRepository>().Setup(x => x.UpdateInvoice(It.IsAny<Invoice>())).Verifiable();

            var service = mock.Create<InvoiceService>();

            var result = service.PatchInvoice(new JsonPatchDocument<InvoicePatchRequest>(), It.IsAny<long>());

            Assert.IsTrue(result != null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void CanPatchInvoice_InvoiceDoesNotExist_ThrowsException()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceRepository>().Setup(x => x.GetInvoiceById(It.IsAny<long>(), It.IsAny<bool>())).Returns(It.IsAny<Invoice>()).Verifiable();

            var service = mock.Create<InvoiceService>();

            service.PatchInvoice(new JsonPatchDocument<InvoicePatchRequest>(), It.IsAny<long>());
        }

        [TestMethod]
        public void CanDelete_InvoiceExists_ReturnsVoid()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceRepository>().Setup(x => x.GetInvoiceById(It.IsAny<long>(), It.IsAny<bool>())).Returns(new Invoice()).Verifiable();
            mock.Mock<IInvoiceRepository>().Setup(x => x.DeleteInvoice(It.IsAny<long>())).Verifiable();

            var service = mock.Create<InvoiceService>();

            service.DeleteInvoice(It.IsAny<long>());
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void CanDelete_InvoiceDoesNotExist_ThrowsException()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceRepository>().Setup(x => x.GetInvoiceById(It.IsAny<long>(), It.IsAny<bool>())).Returns(It.IsAny<Invoice>()).Verifiable();

            var service = mock.Create<InvoiceService>();

            service.DeleteInvoice(It.IsAny<long>());
        }
    }
}