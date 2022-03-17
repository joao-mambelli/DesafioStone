using Microsoft.AspNetCore.Mvc;
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
        public IActionResult AuthorizeUser([FromBody] UserAuthorizeRequest request)
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
        public IActionResult CreateUser([FromBody] UserCreateRequest request)
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
        public IActionResult GetUserById(long userId)
        {
            var user = UserRepository.GetUserById(userId);

            if (user == null)
                return NotFound(new { message = "Usuário com o id '" + userId + "' não existe." });

            return Ok(user);
        }

        [HttpDelete]
        [Route("{userId}")]
        [Authorize(Roles = "Manager")]
        public IActionResult DeleteUser(long userId)
        {
            var user = UserRepository.GetUserById(userId);

            if (user == null)
                return NotFound(new { message = "Usuário com o id '" + userId + "' não existe." });

            UserRepository.DeleteUser(userId);

            return Ok(new { message = "Usuário com o id '" + userId + "' foi excluído." });
        }

        [HttpPost]
        [Route("{userId}/updatepassword")]
        [Authorize]
        public IActionResult UpdateUserPassword([FromBody] UserUpdatePasswordRequest request, long userId)
        {
            if (long.Parse(User.Claims.FirstOrDefault(i => i.Type == "Id").Value) != userId)
            {
                return Unauthorized(new { message = "Você não possui permissão para alterar a senha do usuário com o id '" + userId + "'." });
            }

            var user = UserRepository.UpdateUserPassword(request, userId);

            return Ok(user);
        }
    }
}
