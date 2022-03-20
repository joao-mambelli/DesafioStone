using DesafioStone.DataContracts;
using DesafioStone.Models;

namespace DesafioStone.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> VerifyPasswordAsync(string username, string password);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByIdAsync(long userId);
        Task<User> CreateUserAsync(UserCreateRequest request);
        Task<User> UpdateUserPasswordAsync(UserUpdatePasswordRequest request, long userId);
        Task DeleteUserAsync(long userId);
    }
}