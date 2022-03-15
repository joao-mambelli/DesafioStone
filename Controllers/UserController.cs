using Microsoft.AspNetCore.Mvc;
using DesafioStone.Models;
using DesafioStone.Repositories;
using DesafioStone.Services;
using Microsoft.AspNetCore.Authorization;

namespace DesafioStone.Controllers
{
    [ApiController]
    [Route("v1/user")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("authorize")]
        public ActionResult<dynamic> Authorize([FromBody] User model)
        {
            var user = UserRepository.Get(model.Username, model.Password);

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);

            user.Password = "";

            return new
            {
                user = user,
                token = token,
            };
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult<string> Post([FromBody] User model)
        {
            return "test";
        }

        [HttpGet]
        [Route("{userId}")]
        [Authorize(Roles = "Manager")]
        public ActionResult<string> Get()
        {
            return "test";
        }
    }
}
