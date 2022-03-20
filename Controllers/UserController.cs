﻿using Microsoft.AspNetCore.Mvc;
using DesafioStone.Repositories;
using DesafioStone.Services;
using Microsoft.AspNetCore.Authorization;
using DesafioStone.DataContracts;
using Swashbuckle.AspNetCore.Annotations;
using DesafioStone.Interfaces.Services;
using DesafioStone.Providers;

namespace DesafioStone.Controllers
{
    [ApiController]
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController()
        {
            _service = new UserService(new UserRepository());
        }

        [HttpPost]
        [Route("authorize")]
        [SwaggerOperation(Summary = "In case Username and Passwords are right, retrieves a new 8 hours token.")]
        public async Task<IActionResult> AuthorizeUserAsync([FromBody] UserAuthorizeRequest request)
        {
            try
            {
                var user = await _service.VerifyPasswordAsync(request.Username, request.Password);

                var token = new TokenProvider().GenerateToken(user);

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

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [SwaggerOperation(Summary = "In case no user with same Username exists, creates a new User.")]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserCreateRequest request)
        {
            try
            {
                var user = await _service.CreateUserAsync(request);

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

        [HttpGet]
        [Route("{userId}")]
        [Authorize(Roles = "Manager")]
        [SwaggerOperation(Summary = "In case an user with given Id exists, retrieves it.")]
        public async Task<IActionResult> GetUserByIdAsync(long userId)
        {
            try
            {
                var user = await _service.GetUserByIdAsync(userId);

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
        public async Task<IActionResult> DeleteUserAsync(long userId)
        {
            try
            {
                await _service.DeleteUserAsync(userId);

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

                var user = await _service.UpdateUserPasswordAsync(request, userId);

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
    }
}
