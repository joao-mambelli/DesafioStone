using Microsoft.AspNetCore.Mvc;
using DesafioStone.Models;
using DesafioStone.Repositories;
using DesafioStone.Services;

namespace DesafioStone.Controllers
{
    [ApiController]
    [Route("v1/login")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public ActionResult<dynamic> Login([FromBody] User model)
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
    }
}
