using DesafioStone.DataContracts;
using DesafioStone.Models;

namespace DesafioStone.Interfaces.Services
{
    public interface IUserService
    {
        User VerifyPassword(string username, string password);
        User GetUserByUsername(string username);
        User GetUserById(long userId);
        User CreateUser(UserCreateRequest request);
        User UpdateUserPassword(UserUpdatePasswordRequest request, long userId);
        void LogoutAllDevices(long userId);
        void DeleteUser(long userId);
    }
}