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
using System.Collections.Generic;
using DesafioStone.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace DesafioStone.UnitTests.Controllers
{
    [TestClass]
    public class InvoiceControllerTests
    {
        [TestMethod]
        public void CanGetInvoices_Returns200()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceService>().Setup(x => x.GetInvoices(It.IsAny<InvoiceQuery>())).Returns(new List<Invoice>()).Verifiable();
            mock.Mock<IInvoiceService>().Setup(x => x.GetNumberOfInvoices()).Returns(It.IsAny<long>()).Verifiable();

            var controller = mock.Create<InvoiceController>();

            var result = controller.GetInvoices(new InvoiceQuery());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 200);
        }

        [TestMethod]
        public void CanGetInvoiceById_InvoiceExists_Returns200()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceService>().Setup(x => x.GetInvoiceById(It.IsAny<long>())).Returns(It.IsAny<Invoice>()).Verifiable();

            var controller = mock.Create<InvoiceController>();

            var result = controller.GetInvoiceById(It.IsAny<long>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 200);
        }

        [TestMethod]
        public void CanGetInvoiceById_InvoiceDoesNotExist_Returns404()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceService>().Setup(x => x.GetInvoiceById(It.IsAny<long>())).Throws(NotFound()).Verifiable();

            var controller = mock.Create<InvoiceController>();

            var result = controller.GetInvoiceById(It.IsAny<long>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 404);
        }

        [TestMethod]
        public void CanCreateInvoice_Returns201()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceService>().Setup(x => x.CreateInvoice(It.IsAny<InvoiceCreateRequest>())).Returns(new Invoice()).Verifiable();

            var controller = mock.Create<InvoiceController>();

            var result = controller.CreateInvoice(It.IsAny<InvoiceCreateRequest>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 201);
        }

        [TestMethod]
        public void CanUpdateInvoice_InvoiceExists_Returns200()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceService>().Setup(x => x.UpdateInvoice(It.IsAny<InvoiceUpdateRequest>(), It.IsAny<long>())).Returns(It.IsAny<Invoice>()).Verifiable();

            var controller = mock.Create<InvoiceController>();

            var result = controller.UpdateInvoice(It.IsAny<InvoiceUpdateRequest>(), It.IsAny<long>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 200);
        }

        [TestMethod]
        public void CanUpdateInvoice_InvoiceDoesNotExist_Returns404()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceService>().Setup(x => x.UpdateInvoice(It.IsAny<InvoiceUpdateRequest>(), It.IsAny<long>())).Throws(NotFound()).Verifiable();

            var controller = mock.Create<InvoiceController>();

            var result = controller.UpdateInvoice(It.IsAny<InvoiceUpdateRequest>(), It.IsAny<long>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 404);
        }

        [TestMethod]
        public void CanPatchInvoice_InvoiceExists_Returns200()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceService>().Setup(x => x.PatchInvoice(It.IsAny<JsonPatchDocument<InvoicePatchRequest>>(), It.IsAny<long>())).Returns(It.IsAny<Invoice>()).Verifiable();

            var controller = mock.Create<InvoiceController>();

            var result = controller.PatchInvoice(It.IsAny<JsonPatchDocument<InvoicePatchRequest>>(), It.IsAny<long>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 200);
        }

        [TestMethod]
        public void CanPatchInvoice_InvoiceDoesNotExist_Returns404()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceService>().Setup(x => x.PatchInvoice(It.IsAny<JsonPatchDocument<InvoicePatchRequest>>(), It.IsAny<long>())).Throws(NotFound()).Verifiable();

            var controller = mock.Create<InvoiceController>();

            var result = controller.PatchInvoice(It.IsAny<JsonPatchDocument<InvoicePatchRequest>>(), It.IsAny<long>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 404);
        }

        [TestMethod]
        public void CanDeleteInvoice_InvoiceExists_Returns200()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceService>().Setup(x => x.DeleteInvoice(It.IsAny<long>())).Verifiable();

            var controller = mock.Create<InvoiceController>();

            var result = controller.DeleteInvoice(It.IsAny<long>());

            Assert.IsTrue(((ObjectResult)result).StatusCode == 200);
        }

        [TestMethod]
        public void CanDeleteInvoice_InvoiceDoesNotExist_Returns404()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IInvoiceService>().Setup(x => x.DeleteInvoice(It.IsAny<long>())).Throws(NotFound()).Verifiable();

            var controller = mock.Create<InvoiceController>();

            var result = controller.DeleteInvoice(It.IsAny<long>());

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
