using Microsoft.AspNetCore.Mvc;
using DesafioStone.Models;
using DesafioStone.Repositories;
using DesafioStone.Services;
using Microsoft.AspNetCore.Authorization;
using DesafioStone.DataContracts;

namespace DesafioStone.Controllers
{
    [ApiController]
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("authorize")]
        public ActionResult<dynamic> Authorize([FromBody] UserAuthorizeRequest request)
        {
            var user = UserRepository.VerifyPassword(request.Username, request.Password);

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);

            user.Password = "";

            return new
            {
                user,
                token,
            };
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult<dynamic> Create([FromBody] UserCreateRequest request)
        {
            var user = UserRepository.UsernameExists(request.Username);

            if (user != null)
                return Conflict(new { message = "Usuário \"" + request.Username +  "\" já existe." });

            user = UserRepository.Create(request);

            return user;
        }

        [HttpGet]
        [Route("{userId}")]
        [Authorize(Roles = "Manager")]
        public ActionResult<dynamic> Get(int userId)
        {
            var user = UserRepository.GetUser(userId);

            if (user == null)
                return NotFound(new { message = "Usuário com o id \"" + userId + "\" não existe." });

            return user;
        }
    }
}
