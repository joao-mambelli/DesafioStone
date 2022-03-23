using DesafioStone.Models;

namespace DesafioStone.Interfaces.Providers
{
    public interface ITokenProvider
    {
        string GenerateToken(User user);
    }
}
