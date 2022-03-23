using DesafioStone.Models;

namespace DesafioStone.Interfaces.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers(bool active = true);
        User GetUserById(long userId, bool active = true);
        User GetUserByUsername(string username, bool active = true);
        long InsertUser(User user);
        void UpdateUser(User user);
        void DeleteUser(long userId);
    }
}