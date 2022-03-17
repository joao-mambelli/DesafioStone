using Microsoft.AspNetCore.Mvc;
using DesafioStone.Repositories;
using Microsoft.AspNetCore.Authorization;
using DesafioStone.DataContracts;
using Microsoft.AspNetCore.JsonPatch;
using DesafioStone.Utils.Common;
using DesafioStone.Entities;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;

namespace DesafioStone.Controllers
{
    [ApiController]
    [Route("v1/invoices")]
    public class InvoiceController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Retrieves all Invoices.")]
        public async Task<IActionResult> GetAllInvoicesAsync()
        {
            var invoices = await InvoiceRepository.GetAllActiveInvoicesAsync();

            return Ok(invoices);
        }

        [HttpGet]
        [Authorize]
        [Route("pagination")]
        [SwaggerOperation(Summary = "Retrieves Invoices from specified page.")]
        public async Task<IActionResult> GetPaginatedInvoicesAsync([FromQuery] InvoicePaginationQuery query)
        {
            var invoices = await InvoiceRepository.GetActivePaginatedInvoicesAsync(query);

            var count = await InvoiceRepository.GetNumberOfActiveInvoicesAsync();

            if (invoices == null || count == null)
                return NotFound();

            var meta = new PaginationMetadata(count ?? 0, query.Page, query.RowsPerPage, invoices.Count());

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));

            return Ok(invoices);
        }

        [HttpGet]
        [Route("{invoiceId}")]
        [Authorize]
        [SwaggerOperation(Summary = "In case an Invoice with given Id exists, retrieves it.")]
        public async Task<IActionResult> GetInvoiceByIdAsync(long invoiceId)
        {
            var invoice = await InvoiceRepository.GetInvoiceByIdAsync(invoiceId);

            if (invoice == null)
                return NotFound(new { message = "Nota fiscal com o id '" + invoiceId + "' não existe." });

            return Ok(invoice);
        }

        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Create an Invoice with a new Id.")]
        public async Task<IActionResult> CreateInvoiceAsync([FromBody] InvoiceCreateRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var invoice = await InvoiceRepository.CreateInvoiceAsync(request);

            return Created("v1/invoices/" + invoice.Id, invoice);
        }

        [HttpPut]
        [Route("{invoiceId}")]
        [Authorize]
        [SwaggerOperation(Summary = "In case an Invoice with given Id exists, update all its fields.")]
        public async Task<IActionResult> UpdateInvoiceAsync([FromBody] InvoiceUpdateRequest request, long invoiceId)
        {
            var invoice = await InvoiceRepository.GetInvoiceByIdAsync(invoiceId);

            if (invoice == null)
                return NotFound(new { message = "Nota fiscal com o id '" + invoiceId + "' não existe." });

            invoice = await InvoiceRepository.UpdateInvoiceAsync(request, invoiceId);

            return Ok(invoice);
        }

        [HttpPatch]
        [Route("{invoiceId}")]
        [Authorize]
        [SwaggerOperation(Summary = "In case an Invoice with given Id exists, patches one or more fields of it.", Description = "This is using Microsoft.AspNetCore.JsonPatch.JsonPatchDocument way of patching. Basically you need to provide a list of operations in the body. For example, you can replace Amount and Description giving the following body:\n\n\t[\n\n\t\t{\n\n\t\t\t\"op\": \"replace\",\n\n\t\t\t\"path\": \"amount\",\n\n\t\t\t\"value\": 100\n\n\t\t},\n\n\t\t{\n\n\t\t\t\"op\": \"replace\",\n\n\t\t\t\"path\": \"description\",\n\n\t\t\t\"value\": \"Description example\"\n\n\t\t}\n\n\t]\n\nReference: <a href=\"https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-6.0\">https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-6.0</a>")]
        public async Task<IActionResult> PatchInvoiceAsync([FromBody] JsonPatchDocument<InvoicePatchRequest> request, long invoiceId)
        {
            var invoice = await InvoiceRepository.GetInvoiceByIdAsync(invoiceId);

            if (invoice == null)
                return NotFound(new { message = "Nota fiscal com o id '" + invoiceId + "' não existe." });

            var invoicePatch = Helpers.PatchRequestInvoice(invoice);

            request.ApplyTo(invoicePatch, ModelState);

            invoice = await InvoiceRepository.PatchInvoiceAsync(invoicePatch, invoiceId);

            return Ok(invoice);
        }

        [HttpDelete]
        [Route("{invoiceId}")]
        [Authorize]
        [SwaggerOperation(Summary = "In case an Invoice with given Id exists, marks it as deleted.")]
        public async Task<IActionResult> DeleteInvoiceAsync(long invoiceId)
        {
            var invoice = await InvoiceRepository.GetInvoiceByIdAsync(invoiceId);

            if (invoice == null)
                return NotFound(new { message = "Nota fiscal com o id '" + invoiceId + "' não existe." });

            await InvoiceRepository.DeleteInvoiceAsync(invoiceId);

            return Ok(new { message = "Nota fiscal com o id '" + invoiceId + "' foi excluída." });
        }
    }
}