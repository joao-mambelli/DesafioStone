using DesafioStone.DataContracts;
using DesafioStone.Interfaces.ModelsInterfaces;

namespace DesafioStone.Interfaces.ServicesInterfaces
{
    public interface IUserService
    {
        Task<IUser> VerifyPasswordAsync(string username, string password);
        Task<IUser> GetUserByUsernameAsync(string username);
        Task<IUser> GetUserByIdAsync(long userId);
        Task<IUser> CreateUserAsync(UserCreateRequest request);
        Task<IUser> UpdateUserPasswordAsync(UserUpdatePasswordRequest request, long userId);
        Task DeleteUserAsync(long userId);
    }
}