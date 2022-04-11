using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using DesafioStone.Models;
using DesafioStone.Interfaces.Providers;
using DesafioStone.Interfaces.Services;
using System.Security.Cryptography;
using DesafioStone.Interfaces.Repositories;
using DesafioStone.Utils.Common;
using System.Net;

namespace DesafioStone.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly ISecretProvider _accessSecretProvider;
        private readonly ITokenRepository _repository;

        public TokenService(JwtSecurityTokenHandler tokenHandler, ISecretProvider accessSecretProvider, ITokenRepository repository)
        {
            _tokenHandler = tokenHandler;
            _accessSecretProvider = accessSecretProvider;
            _repository = repository;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("Id", user.Id.ToString())
            };

            return GenerateToken(claims);
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var key = Encoding.ASCII.GetBytes(_accessSecretProvider.Secret());
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(claims)
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);

            return _tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public string GenerateAndSaveRefreshToken(long? userId)
        {
            if (userId == null)
            {
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "Invalid user id.");
            }

            if (_repository.GetRefreshTokenById(userId.Value) != null)
            {
                DeleteRefreshToken(userId.Value);
            }

            var refreshToken = GenerateRefreshToken();

            InsertRefreshToken(userId.Value, refreshToken);

            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromValidToken(string token)
        {
            if (IsTokenExpired(token))
            {
                throw new SecurityTokenException("Expired token");
            }

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_accessSecretProvider.Secret())),
                ValidateLifetime = false
            };

            var principal = _tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public void InsertRefreshToken(long userId, string refreshToken)
        {
            _repository.InsertRefreshToken(userId, refreshToken);
        }

        public string GetRefreshTokenById(long userId)
        {
            var refreshToken = _repository.GetRefreshTokenById(userId);

            if (refreshToken == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "Refresh token not found.");

            return refreshToken;
        }

        public void DeleteRefreshToken(long userId)
        {
            var invoice = _repository.GetRefreshTokenById(userId);

            if (invoice == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "Refresh token not found.");

            _repository.DeleteRefreshToken(userId);
        }

        public static bool IsTokenExpired(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return true;
            }

            var indexOfFirstPoint = token.IndexOf('.') + 1;
            var toDecode = token[indexOfFirstPoint..token.LastIndexOf('.')];
            while (toDecode.Length % 4 != 0)
            {
                toDecode += '=';
            }

            string decodedString = Encoding.ASCII.GetString(Convert.FromBase64String(toDecode));

            var beginning = "\"exp\":";
            var startPosition = decodedString.LastIndexOf(beginning) + beginning.Length;
            decodedString = decodedString[startPosition..];
            var endPosition = decodedString.IndexOf("\"") - 1;
            decodedString = decodedString[..endPosition];
            var timestamp = Convert.ToInt64(decodedString);

            var date = new DateTime(1970, 1, 1).AddSeconds(timestamp);

            int result = DateTime.Compare(date, DateTime.UtcNow);

            return result < 0;
        }
    }
}
