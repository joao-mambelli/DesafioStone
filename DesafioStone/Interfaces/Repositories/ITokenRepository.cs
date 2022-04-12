namespace DesafioStone.Interfaces.Repositories
{
    public interface ITokenRepository
    {
        string GetRefreshTokenByUserId(long userId);
        void InsertRefreshToken(long userId, string refreshToken);
        void DeleteRefreshToken(long userId);
    }
}