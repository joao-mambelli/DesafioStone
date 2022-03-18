using DesafioStone.Interfaces.ModelsInterfaces;

namespace DesafioStone.Interfaces.RepositoriesInterfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<IUser>> GetAllUsersAsync(bool active = true);
        Task<IUser> GetUserByIdAsync(long userId, bool active = true);
        Task<IUser> GetUserByUsernameAsync(string username, bool active = true);
        Task<long> InsertUserAsync(IUser user);
        Task UpdateUserAsync(IUser user);
        Task DeleteUserAsync(long userId);
    }
}