using DesafioStone.Models;
using System.Security.Claims;

namespace DesafioStone.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        string GenerateToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        string GenerateAndSaveRefreshToken(long? userId);
        ClaimsPrincipal GetPrincipalFromValidToken(string token);
        void InsertRefreshToken(long userId, string refreshToken);
        string GetRefreshTokenById(long userId);
        void DeleteRefreshToken(long userId);
    }
}
