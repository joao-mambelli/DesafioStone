using Microsoft.AspNetCore.Mvc;
using DesafioStone.Repositories;
using DesafioStone.Services;
using Microsoft.AspNetCore.Authorization;
using DesafioStone.DataContracts;
using Swashbuckle.AspNetCore.Annotations;
using DesafioStone.Utils.Common;

namespace DesafioStone.Controllers
{
    [ApiController]
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        private readonly CustomResults _customResults = new();

        [HttpPost]
        [Route("authorize")]
        [SwaggerOperation(Summary = "In case Username and Passwords are right, retrieves a new 8 hours token.")]
        public async Task<IActionResult> AuthorizeUserAsync([FromBody] UserAuthorizeRequest request)
        {
            try
            {
                var user = await UserRepository.VerifyPasswordAsync(request.Username, request.Password);

                if (user.Exception != null)
                    return _customResults.InternalServerError(user.Exception);

                if (user == null)
                    return NotFound(new { message = "Invalid username or password." });

                var token = TokenService.GenerateToken(user.Object);

                return Ok(new
                {
                    user.Object,
                    token,
                });
            }
            catch (Exception ex)
            {
                return _customResults.InternalServerError(ex);
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

                if (user.Exception != null)
                    return _customResults.InternalServerError(user.Exception);

                if (user.Object != null)
                    return Conflict(new { message = "Username '" + request.Username +  "' already exists." });

                user = await UserRepository.CreateUserAsync(request);

                return Created("v1/users/" + user.Object.Id, user.Object);
            }
            catch (Exception ex)
            {
                return _customResults.InternalServerError(ex);
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

                if (user.Exception != null)
                    return _customResults.InternalServerError(user.Exception);

                if (user.Object == null)
                    return NotFound(new { message = "User with id '" + userId + "' do not exist." });

                return Ok(user.Object);
            }
            catch (Exception ex)
            {
                return _customResults.InternalServerError(ex);
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

                if (user.Exception != null)
                    return _customResults.InternalServerError(user.Exception);

                if (user.Object == null)
                    return NotFound(new { message = "User with id '" + userId + "' do not exist." });

                var exeption = await UserRepository.DeleteUserAsync(userId);

                if (exeption != null)
                    return _customResults.InternalServerError(exeption);

                return Ok(new { message = "User with id '" + userId + "' was marked as deleted." });
            }
            catch (Exception ex)
            {
                return _customResults.InternalServerError(ex);
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

                if (user.Exception != null)
                    return _customResults.InternalServerError(user.Exception);

                return Ok(user.Object);
            }
            catch (Exception ex)
            {
                return _customResults.InternalServerError(ex);
            }
        }
    }
}
