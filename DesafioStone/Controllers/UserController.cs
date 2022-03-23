using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DesafioStone.DataContracts;
using Swashbuckle.AspNetCore.Annotations;
using DesafioStone.Interfaces.Services;
using DesafioStone.Interfaces.Providers;

namespace DesafioStone.Controllers
{
    [ApiController]
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ITokenProvider _tokenProvider;

        public UserController(IUserService service, ITokenProvider tokenProvider)
        {
            _service = service;
            _tokenProvider = tokenProvider;
        }

        [HttpPost]
        [Route("authorize")]
        [SwaggerOperation(Summary = "In case Username and Passwords are right, retrieves a new 8 hours token.")]
        public IActionResult AuthorizeUser([FromBody] UserAuthorizeRequest request)
        {
            try
            {
                var user = _service.VerifyPassword(request.Username, request.Password);

                var token = _tokenProvider.GenerateToken(user);

                return Ok(new
                {
                    user,
                    token,
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
        [Route("{userId}")]
        [Authorize(Roles = "Manager")]
        [SwaggerOperation(Summary = "In case an user with given Id exists, retrieves it.")]
        public IActionResult GetUserById(long userId)
        {
            try
            {
                var user = _service.GetUserById(userId);

                return Ok(user);
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
        [Authorize(Roles = "Manager")]
        [SwaggerOperation(Summary = "In case no user with same Username exists, creates a new User.")]
        public IActionResult CreateUser([FromBody] UserCreateRequest request)
        {
            try
            {
                var user = _service.CreateUser(request);

                return Created("v1/users/" + user.Id, user);
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
        [Route("{userId}/updatepassword")]
        [Authorize]
        [SwaggerOperation(Summary = "In case an user with given Id exists and it's the same Id as the one stored in the token, changes its password.")]
        public IActionResult UpdateUserPassword([FromBody] UserUpdatePasswordRequest request, long userId)
        {
            try
            {
                if (long.Parse(User.Claims.FirstOrDefault(i => i.Type == "Id").Value) != userId)
                {
                    return Unauthorized(new { message = "You do not have permission to change this user's password." });
                }

                var user = _service.UpdateUserPassword(request, userId);

                return Ok(user);
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
        [Route("{userId}")]
        [Authorize(Roles = "Manager")]
        [SwaggerOperation(Summary = "In case an user with given Id exists, marks it as deleted.")]
        public IActionResult DeleteUser(long userId)
        {
            try
            {
                _service.DeleteUser(userId);

                return Ok(new { message = "User with id '" + userId + "' was marked as deleted." });
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
