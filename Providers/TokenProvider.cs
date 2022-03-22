using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using DesafioStone.Utils.Common;
using DesafioStone.Models;
using DesafioStone.Interfaces.Providers;

namespace DesafioStone.Providers
{
    public class TokenProvider : ITokenProvider
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public TokenProvider(JwtSecurityTokenHandler tokenHandler)
        {
            _tokenHandler = tokenHandler;
        }

        public string GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(AccessSecret.Secret());
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim("Id", user.Id.ToString())
                })
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);

            return _tokenHandler.WriteToken(token);
        }
    }
}
