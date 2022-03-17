using Microsoft.AspNetCore.Mvc;
using DesafioStone.Models;
using Microsoft.AspNetCore.Authorization;
using DesafioStone.DataContracts;

namespace DesafioStone.Controllers
{
    [ApiController]
    [Route("v1/invoices")]
    public class InvoiceController : ControllerBase
    {
        /*private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(ILogger<InvoiceController> logger)
        {
            _logger = logger;
        }*/

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<string>> ReadAll()
        {
            var list = new List<string>
            {
                "test1",
                "test2"
            };

            return list;
        }

        [HttpGet]
        [Route("{invoiceId}")]
        [Authorize]
        public ActionResult<string> ReadById(int invoiceId)
        {
            return "test";
        }

        [HttpPost]
        [Authorize]
        public ActionResult<string> Create([FromBody] InvoiceCreateRequest request)
        {
            return "test";
        }

        [HttpPut]
        [Route("{invoiceId}")]
        [Authorize]
        public ActionResult<string> Update([FromBody] InvoiceUpdateRequest request, int invoiceId)
        {
            return "test";
        }

        [HttpPatch]
        [Route("{invoiceId}")]
        [Authorize]
        public ActionResult<string> Patch([FromBody] InvoicePatchRequest request, int invoiceId)
        {
            return "test";
        }

        [HttpDelete]
        [Route("{invoiceId}")]
        [Authorize]
        public ActionResult<string> Delete(int invoiceId)
        {
            return "test";
        }
    }
}