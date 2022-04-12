using Microsoft.AspNetCore.Mvc;
using DesafioStone.DataContracts;
using Swashbuckle.AspNetCore.Annotations;
using DesafioStone.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using DesafioStone.Filters;

namespace DesafioStone.Controllers
{
    [ApiController]
    [Route("v1/login")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public LoginController(IUserService service, ITokenService tokenService)
        {
            _userService = service;
            _tokenService = tokenService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "In case Username and Passwords are right, retrieves a new 8 hours token.")]
        public IActionResult Login([FromBody] UserAuthorizeRequest request)
        {
            try
            {
                var user = _userService.VerifyPassword(request.Username, request.Password);

                var token = _tokenService.GenerateToken(user);
                var refreshToken = _tokenService.GenerateAndSaveRefreshToken(user?.Id);

                if (user != null)
                {
                    user.Password = "";
                }

                return Ok(new
                {
                    user,
                    token,
                    refreshToken
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
        [Route("refresh")]
        [SwaggerOperation(Summary = "Refreshes the user's token if the given token is still valid and the refresh token is right.", Description = "Require authorization.")]
        [TokenAuthenticationFilter]
        public IActionResult Refresh([FromBody] UserRefreshRequest request)
        {
            try
            {
                var userId = long.Parse(HttpContext.Request.Headers.First(x => x.Key == "UserId").Value);

                var savedRefreshToken = _tokenService.GetRefreshTokenByUserId(userId);
                if (savedRefreshToken != request.RefreshToken)
                {
                    throw new SecurityTokenException("Invalid refresh token");
                }

                var user = _userService.GetUserById(userId);

                var token = _tokenService.GenerateToken(user);
                var refreshToken = _tokenService.GenerateAndSaveRefreshToken(userId);

                return Ok(new
                {
                    token,
                    refreshToken
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
        [Route("logoutalldevices")]
        [SwaggerOperation(Summary = "Performs a logout from all devices by changing the secret used to sign the token, making all previous tokens invalid.", Description = "Require authorization.")]
        [TokenAuthenticationFilter]
        public IActionResult LogoutAllDevices()
        {
            try
            {
                var userId = long.Parse(HttpContext.Request.Headers.First(x => x.Key == "UserId").Value);

                _userService.LogoutAllDevices(userId);

                return Ok();
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
