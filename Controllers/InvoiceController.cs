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
        private readonly CustomResults _customResults = new();

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Retrieves all Invoices.")]
        public async Task<IActionResult> GetAllInvoicesAsync()
        {
            try
            {
                var invoices = await InvoiceRepository.GetAllActiveInvoicesAsync();

                if (invoices.Exception != null)
                    return _customResults.InternalServerError(invoices.Exception);

                return Ok(invoices.Object);
            }
            catch (Exception ex)
            {
                return _customResults.InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("pagination")]
        [SwaggerOperation(Summary = "Retrieves Invoices from specified page.")]
        public async Task<IActionResult> GetPaginatedInvoicesAsync([FromQuery] InvoicePaginationQuery query)
        {
            try
            {
                var invoices = await InvoiceRepository.GetActivePaginatedInvoicesAsync(query);

                if (invoices.Exception != null)
                    return _customResults.InternalServerError(invoices.Exception);

                var count = await InvoiceRepository.GetNumberOfActiveInvoicesAsync();

                if (count.Exception != null)
                    return _customResults.InternalServerError(count.Exception);

                var meta = new PaginationMetadata(count.Object ?? 0, query.Page, query.RowsPerPage, invoices.Object.Count());

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));

                return Ok(invoices.Object);
            }
            catch (Exception ex)
            {
                return _customResults.InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{invoiceId}")]
        [Authorize]
        [SwaggerOperation(Summary = "In case an Invoice with given Id exists, retrieves it.")]
        public async Task<IActionResult> GetInvoiceByIdAsync(long invoiceId)
        {
            try
            {
                var invoice = await InvoiceRepository.GetInvoiceByIdAsync(invoiceId);

                if (invoice.Exception != null)
                    return _customResults.InternalServerError(invoice.Exception);

                if (invoice.Object == null)
                    return NotFound(new { message = "Invoice with id '" + invoiceId + "' do not exist." });

                return Ok(invoice.Object);
            }
            catch (Exception ex)
            {
                return _customResults.InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Create an Invoice with a new Id.")]
        public async Task<IActionResult> CreateInvoiceAsync([FromBody] InvoiceCreateRequest request)
        {
            try
            {
                var invoice = await InvoiceRepository.CreateInvoiceAsync(request);

                if (invoice.Exception != null)
                    return _customResults.InternalServerError(invoice.Exception);

                return Created("v1/invoices/" + invoice.Object.Id, invoice.Object);
            }
            catch (Exception ex)
            {
                return _customResults.InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{invoiceId}")]
        [Authorize]
        [SwaggerOperation(Summary = "In case an Invoice with given Id exists, update all its fields.")]
        public async Task<IActionResult> UpdateInvoiceAsync([FromBody] InvoiceUpdateRequest request, long invoiceId)
        {
            try
            {
                var invoice = await InvoiceRepository.GetInvoiceByIdAsync(invoiceId);

                if (invoice.Exception != null)
                    return _customResults.InternalServerError(invoice.Exception);

                if (invoice.Object == null)
                    return NotFound(new { message = "Invoice with id '" + invoiceId + "' do not exist." });

                invoice = await InvoiceRepository.UpdateInvoiceAsync(request, invoiceId);

                if (invoice.Exception != null)
                    return _customResults.InternalServerError(invoice.Exception);

                return Ok(invoice.Object);
            }
            catch (Exception ex)
            {
                return _customResults.InternalServerError(ex);
            }
        }

        [HttpPatch]
        [Route("{invoiceId}")]
        [Authorize]
        [SwaggerOperation(Summary = "In case an Invoice with given Id exists, patches one or more fields of it.", Description = "This is using Microsoft.AspNetCore.JsonPatch.JsonPatchDocument way of patching. Basically you need to provide a list of operations in the body. For example, you can replace Amount and Description giving the following body:\n\n\t[\n\n\t\t{\n\n\t\t\t\"op\": \"replace\",\n\n\t\t\t\"path\": \"amount\",\n\n\t\t\t\"value\": 100\n\n\t\t},\n\n\t\t{\n\n\t\t\t\"op\": \"replace\",\n\n\t\t\t\"path\": \"description\",\n\n\t\t\t\"value\": \"Description example\"\n\n\t\t}\n\n\t]\n\nReference: <a href=\"https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-6.0\">https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-6.0</a>")]
        public async Task<IActionResult> PatchInvoiceAsync([FromBody] JsonPatchDocument<InvoicePatchRequest> request, long invoiceId)
        {
            try
            {
                var invoice = await InvoiceRepository.GetInvoiceByIdAsync(invoiceId);

                if (invoice.Exception != null)
                    return _customResults.InternalServerError(invoice.Exception);

                if (invoice.Object == null)
                    return NotFound(new { message = "Invoice with id '" + invoiceId + "' do not exist." });

                var invoicePatch = Helpers.PatchRequestInvoice(invoice.Object);

                request.ApplyTo(invoicePatch, ModelState);

                invoice = await InvoiceRepository.PatchInvoiceAsync(invoicePatch, invoiceId);

                if (invoice.Exception != null)
                    return _customResults.InternalServerError(invoice.Exception);

                return Ok(invoice.Object);
            }
            catch (Exception ex)
            {
                return _customResults.InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{invoiceId}")]
        [Authorize]
        [SwaggerOperation(Summary = "In case an Invoice with given Id exists, marks it as deleted.")]
        public async Task<IActionResult> DeleteInvoiceAsync(long invoiceId)
        {
            try
            {
                var invoice = await InvoiceRepository.GetInvoiceByIdAsync(invoiceId);

                if (invoice.Exception != null)
                    return _customResults.InternalServerError(invoice.Exception);

                if (invoice.Object == null)
                    return NotFound(new { message = "Invoice with id '" + invoiceId + "' do not exist." });

                var exception = await InvoiceRepository.DeleteInvoiceAsync(invoiceId);

                if (exception != null)
                    return _customResults.InternalServerError(invoice.Exception);

                return Ok(new { message = "Invoice with id '" + invoiceId + "' was marked as deleted." });
            }
            catch (Exception ex)
            {
                return _customResults.InternalServerError(ex);
            }
        }
    }
}