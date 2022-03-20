using DesafioStone.Models;

namespace DesafioStone.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync(bool active = true);
        Task<User> GetUserByIdAsync(long userId, bool active = true);
        Task<User> GetUserByUsernameAsync(string username, bool active = true);
        Task<long> InsertUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(long userId);
    }
}