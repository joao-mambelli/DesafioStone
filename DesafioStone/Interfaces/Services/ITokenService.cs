using DesafioStone.Models;
using System.Security.Claims;

namespace DesafioStone.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        string GenerateToken(IEnumerable<Claim> claims, string secret);
        ClaimsPrincipal ValidateToken(string token, long userId);
        string GenerateAndSaveRefreshToken(long? userId);
        void InsertRefreshToken(long userId, string refreshToken);
        string GetRefreshTokenByUserId(long userId);
        void DeleteRefreshToken(long userId);
    }
}
