using Microsoft.AspNetCore.Mvc;
using DesafioStone.Repositories;
using Microsoft.AspNetCore.Authorization;
using DesafioStone.DataContracts;
using Microsoft.AspNetCore.JsonPatch;
using DesafioStone.Entities;
using Swashbuckle.AspNetCore.Annotations;
using DesafioStone.Interfaces.ServicesInterfaces;
using DesafioStone.Services;

namespace DesafioStone.Controllers
{
    [ApiController]
    [Route("v1/invoices")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _service;

        public InvoiceController()
        {
            _service = new InvoiceService(new InvoiceRepository());
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Retrieves all Invoices.")]
        public async Task<IActionResult> GetAllInvoicesAsync()
        {
            try
            {
                var invoices = await _service.GetAllInvoicesAsync();

                return Ok(invoices);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, exeption = ex });
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
                var invoices = await _service.GetPaginatedInvoicesAsync(query);

                var count = await _service.GetNumberOfInvoicesAsync();

                var pagination = new PaginationMetadata(count, query.Page, query.RowsPerPage, invoices.Count());

                return Ok(new
                {
                    pagination,
                    invoices,
                });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, exeption = ex });
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
                var invoice = await _service.GetInvoiceByIdAsync(invoiceId);

                return Ok(invoice);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, exeption = ex });
            }
        }

        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Create an Invoice with a new Id.")]
        public async Task<IActionResult> CreateInvoiceAsync([FromBody] InvoiceCreateRequest request)
        {
            try
            {
                var invoice = await _service.CreateInvoiceAsync(request);

                return Created("v1/invoices/" + invoice.Id, invoice);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, exeption = ex });
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
                var invoice = await _service.UpdateInvoiceAsync(request, invoiceId);

                return Ok(invoice);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, exeption = ex });
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
                var invoice = await _service.PatchInvoiceAsync(request, invoiceId);

                return Ok(invoice);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, exeption = ex });
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
                await _service.DeleteInvoiceAsync(invoiceId);

                return Ok(new { message = "Invoice with id '" + invoiceId + "' was marked as deleted." });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, exeption = ex });
            }
        }
    }
}