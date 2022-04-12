using DesafioStone.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DesafioStone.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TokenAuthenticationFilter : Attribute, IAuthorizationFilter
    {
        public ITokenService TokenService;
        public string Roles;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                context.Result = new ObjectResult("Authorization header not found")
                {
                    StatusCode = 401,
                };
                return;
            }

            if (!context.HttpContext.Request.Headers.ContainsKey("UserId"))
            {
                context.Result = new ObjectResult("UserId header not found")
                {
                    StatusCode = 401,
                };
                return;
            }

            var token = context.HttpContext.Request.Headers.First(x => x.Key == "Authorization").Value;
            var userId = long.Parse(context.HttpContext.Request.Headers.First(x => x.Key == "UserId").Value);

            try
            {
                var claimsPrincipal = TokenService.ValidateToken(token, userId);

                if (!string.IsNullOrWhiteSpace(Roles))
                {
                    var role = claimsPrincipal.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Role).Value;
                    var validRoles = Roles.Split(",");

                    if (validRoles.FirstOrDefault(x => x == role) == null)
                    {
                        context.Result = new ObjectResult("Your Role doesn't match the required roles: " + Roles.Replace(",", ", "))
                        {
                            StatusCode = 401,
                        };
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                context.Result = new ObjectResult(ex.Message)
                {
                    StatusCode = 401,
                };
                return;
            }
        }
    }
}
