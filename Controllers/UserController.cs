using Microsoft.AspNetCore.Mvc;
using DesafioStone.Repositories;
using DesafioStone.Services;
using DesafioStone.Models;
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
        public ActionResult<dynamic> AuthorizeUser([FromBody] UserAuthorizeRequest request)
        {
            var user = UserRepository.VerifyPassword(request.Username, request.Password);

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);

            return Ok(new
            {
                user,
                token,
            });
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult<User> CreateUser([FromBody] UserCreateRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var user = UserRepository.GetUserByUsername(request.Username);

            if (user != null)
                return Conflict(new { message = "Usuário '" + request.Username +  "' já existe." });

            user = UserRepository.CreateUser(request);

            return Created("v1/users/" + user.Id, user);
        }

        [HttpGet]
        [Route("{userId}")]
        [Authorize(Roles = "Manager")]
        public ActionResult<User> GetUser(int userId)
        {
            var user = UserRepository.GetUserById(userId);

            if (user == null)
                return NotFound(new { message = "Usuário com o id '" + userId + "' não existe." });

            return Ok(user);
        }

        [HttpDelete]
        [Route("{userId}")]
        [Authorize(Roles = "Manager")]
        public ActionResult<User> DeleteUser(int userId)
        {
            var user = UserRepository.GetUserById(userId);

            if (user == null)
                return NotFound(new { message = "Usuário com o id '" + userId + "' não existe." });

            UserRepository.DeleteUser(userId);

            return Ok();
        }
    }
}
