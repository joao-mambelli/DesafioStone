using Microsoft.AspNetCore.Mvc;
using DesafioStone.DataContracts;
using Swashbuckle.AspNetCore.Annotations;
using DesafioStone.Interfaces.Services;
using DesafioStone.Filters;

namespace DesafioStone.Controllers
{
    [ApiController]
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{userId}")]
        [TokenAuthenticationFilter(Roles = "Manager")]
        [SwaggerOperation(Summary = "In case an user with given Id exists, retrieves it.", Description = "Require authorization and at least Manager role.")]
        public IActionResult GetUserById(long userId)
        {
            try
            {
                var user = _service.GetUserById(userId);
                if (user != null)
                {
                    user.Password = "";
                }

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
        [TokenAuthenticationFilter(Roles = "Manager")]
        [SwaggerOperation(Summary = "In case no user with same Username exists, creates a new User.", Description = "Require authorization and at least Manager role.")]
        public IActionResult CreateUser([FromBody] UserCreateRequest request)
        {
            try
            {
                var user = _service.CreateUser(request);
                if (user != null)
                {
                    user.Password = "";
                }

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
        [Route("updatepassword")]
        [TokenAuthenticationFilter]
        [SwaggerOperation(Summary = "In case an user with given Id exists, changes its password.", Description = "Require authorization.")]
        public IActionResult UpdateUserPassword([FromBody] UserUpdatePasswordRequest request)
        {
            try
            {
                var userId = long.Parse(HttpContext.Request.Headers.First(x => x.Key == "UserId").Value);

                var user = _service.UpdateUserPassword(request, userId);
                if (user != null)
                {
                    user.Password = "";
                }

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
        [TokenAuthenticationFilter(Roles = "Manager")]
        [SwaggerOperation(Summary = "In case an user with given Id exists, marks it as deleted.", Description = "Require authorization and at least Manager role.")]
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
