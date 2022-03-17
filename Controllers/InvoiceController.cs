using Microsoft.AspNetCore.Mvc;
using DesafioStone.Repositories;
using Microsoft.AspNetCore.Authorization;
using DesafioStone.DataContracts;
using Microsoft.AspNetCore.JsonPatch;
using DesafioStone.Utils.Common;

namespace DesafioStone.Controllers
{
    [ApiController]
    [Route("v1/invoices")]
    public class InvoiceController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult GetAllInvoices()
        {
            var invoices = InvoiceRepository.GetAllInvoices();

            return Ok(invoices);
        }

        [HttpGet]
        [Route("{invoiceId}")]
        [Authorize]
        public IActionResult GetInvoiceById(long invoiceId)
        {
            var invoice = InvoiceRepository.GetInvoiceById(invoiceId);

            if (invoice == null)
                return NotFound(new { message = "Nota fiscal com o id '" + invoiceId + "' não existe." });

            return Ok(invoice);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateInvoice([FromBody] InvoiceCreateRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var invoice = InvoiceRepository.CreateInvoice(request);

            return Created("v1/invoices/" + invoice.Id, invoice);
        }

        [HttpPut]
        [Route("{invoiceId}")]
        [Authorize]
        public IActionResult UpdateInvoice([FromBody] InvoiceUpdateRequest request, long invoiceId)
        {
            var invoice = InvoiceRepository.GetInvoiceById(invoiceId);

            if (invoice == null)
                return NotFound(new { message = "Nota fiscal com o id '" + invoiceId + "' não existe." });

            invoice = InvoiceRepository.UpdateInvoice(request, invoiceId);

            return Ok(invoice);
        }

        [HttpPatch]
        [Route("{invoiceId}")]
        [Authorize]
        public IActionResult PatchInvoice([FromBody] JsonPatchDocument<InvoicePatchRequest> request, long invoiceId)
        {
            var invoice = InvoiceRepository.GetInvoiceById(invoiceId);

            if (invoice == null)
                return NotFound(new { message = "Nota fiscal com o id '" + invoiceId + "' não existe." });

            var invoicePatch = Helpers.PatchRequestInvoice(invoice);

            request.ApplyTo(invoicePatch, ModelState);

            invoice = InvoiceRepository.PatchInvoice(invoicePatch, invoiceId);

            return Ok(invoice);
        }

        [HttpDelete]
        [Route("{invoiceId}")]
        [Authorize]
        public IActionResult DeleteInvoice(long invoiceId)
        {
            var invoice = InvoiceRepository.GetInvoiceById(invoiceId);

            if (invoice == null)
                return NotFound(new { message = "Nota fiscal com o id '" + invoiceId + "' não existe." });

            InvoiceRepository.DeleteInvoice(invoiceId);

            return Ok(new { message = "Nota fiscal com o id '" + invoiceId + "' foi excluída." });
        }
    }
}