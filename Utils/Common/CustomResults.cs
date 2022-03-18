using Microsoft.AspNetCore.Mvc;

namespace DesafioStone.Utils.Common
{
    public class CustomResults : ControllerBase
    {
        public ObjectResult InternalServerError(object obj)
        {
            return StatusCode(500, new { message = "Internal Server Error.", exeption = obj });
        }
    }
}
