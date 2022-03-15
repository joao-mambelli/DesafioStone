using Microsoft.AspNetCore.Mvc;
using DesafioStone.Models;
using Microsoft.AspNetCore.Authorization;

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
        public ActionResult<IEnumerable<string>> GetAll()
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
        public ActionResult<string> GetById()
        {
            return "test";
        }

        [HttpPost]
        [Authorize]
        public ActionResult<string> Post([FromBody] Invoice model)
        {
            return "test";
        }

        [HttpPut]
        [Route("{invoiceId}")]
        [Authorize]
        public ActionResult<string> Put([FromBody] Invoice model)
        {
            return "test";
        }

        [HttpPatch]
        [Route("{invoiceId}")]
        [Authorize]
        public ActionResult<string> Patch([FromBody] Invoice model)
        {
            return "test";
        }

        [HttpDelete]
        [Route("{invoiceId}")]
        [Authorize]
        public ActionResult<string> Delete()
        {
            return "test";
        }
    }
}