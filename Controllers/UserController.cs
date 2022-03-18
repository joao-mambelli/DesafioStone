using Microsoft.AspNetCore.Mvc;
using DesafioStone.Repositories;
using DesafioStone.Services;
using Microsoft.AspNetCore.Authorization;
using DesafioStone.DataContracts;
using Swashbuckle.AspNetCore.Annotations;

namespace DesafioStone.Controllers
{
    [ApiController]
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("authorize")]
        [SwaggerOperation(Summary = "In case Username and Passwords are right, retrieves a new 8 hours token.")]
        public async Task<IActionResult> AuthorizeUserAsync([FromBody] UserAuthorizeRequest request)
        {
            try
            {
                var user = await UserRepository.VerifyPasswordAsync(request.Username, request.Password);

                if (user == null)
                    return NotFound(new { message = "Invalid username or password." });

                var token = TokenService.GenerateToken(user);

                return Ok(new
                {
                    user,
                    token,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error.", exeption = ex });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [SwaggerOperation(Summary = "In case no user with same Username exists, creates a new User.")]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserCreateRequest request)
        {
            try
            {
                var user = await UserRepository.GetUserByUsernameAsync(request.Username);

                if (user != null)
                    return Conflict(new { message = "Username '" + request.Username +  "' already exists." });

                user = await UserRepository.CreateUserAsync(request);

                return Created("v1/users/" + user.Id, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error.", exeption = ex });
            }
        }

        [HttpGet]
        [Route("{userId}")]
        [Authorize(Roles = "Manager")]
        [SwaggerOperation(Summary = "In case an user with given Id exists, retrieves it.")]
        public async Task<IActionResult> GetUserByIdAsync(long userId)
        {
            try
            {
                var user = await UserRepository.GetUserByIdAsync(userId);

                if (user == null)
                    return NotFound(new { message = "User with id '" + userId + "' do not exist." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error.", exeption = ex });
            }
        }

        [HttpDelete]
        [Route("{userId}")]
        [Authorize(Roles = "Manager")]
        [SwaggerOperation(Summary = "In case an user with given Id exists, marks it as deleted.")]
        public async Task<IActionResult> DeleteUserAsync(long userId)
        {
            try
            {
                var user = await UserRepository.GetUserByIdAsync(userId);

                if (user == null)
                    return NotFound(new { message = "User with id '" + userId + "' do not exist." });

                await UserRepository.DeleteUserAsync(userId);

                return Ok(new { message = "User with id '" + userId + "' was marked as deleted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error.", exeption = ex });
            }
        }

        [HttpPost]
        [Route("{userId}/updatepassword")]
        [Authorize]
        [SwaggerOperation(Summary = "In case an user with given Id exists and it's the same Id as the one stored in the token, changes its password.")]
        public async Task<IActionResult> UpdateUserPasswordAsync([FromBody] UserUpdatePasswordRequest request, long userId)
        {
            try
            {
                if (long.Parse(User.Claims.FirstOrDefault(i => i.Type == "Id").Value) != userId)
                {
                    return Unauthorized(new { message = "You do not have permission to change this user's password." });
                }

                var user = await UserRepository.UpdateUserPasswordAsync(request, userId);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error.", exeption = ex });
            }
        }
    }
}
