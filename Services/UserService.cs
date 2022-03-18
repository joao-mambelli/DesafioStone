using DesafioStone.Interfaces.ModelsInterfaces;
using DesafioStone.Interfaces.RepositoriesInterfaces;
using DesafioStone.Interfaces.ServicesInterfaces;
using DesafioStone.Models;
using DesafioStone.DataContracts;
using DesafioStone.Enums;
using DesafioStone.Utils.Common;
using System.Net;
using DesafioStone.Interfaces.ProvidersInterfaces;
using DesafioStone.Providers;

namespace DesafioStone.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IUser> VerifyPasswordAsync(string username, string password)
        {
            var user = await _repository.GetUserByUsernameAsync(username);

            if (user == null && !Password.IsValid(password, user.Password))
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "Invalid username or password.");

            return user;
        }

        public async Task<IUser> GetUserByUsernameAsync(string username)
        {
            var user = await _repository.GetUserByUsernameAsync(username);

            if (user == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "User not found.");

            return user;
        }

        public async Task<IUser> GetUserByIdAsync(long userId)
        {
            var user = await _repository.GetUserByIdAsync(userId);

            if (user == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "User not found.");

            return user;
        }

        public async Task<IUser> CreateUserAsync(UserCreateRequest request)
        {
            IHashProvider hashProvider = new HashProvider();

            var user = await _repository.GetUserByUsernameAsync(request.Username);

            if (user != null)
                throw Helpers.BuildHttpException(HttpStatusCode.Conflict, "Username already exists.");

            var insertedId = await _repository.InsertUserAsync(new User()
            {
                Username = request.Username,
                Password = hashProvider.ComputeHash(request.Password),
                Role = request.Role ?? RoleEnum.User
            });

            return await _repository.GetUserByIdAsync(insertedId);
        }

        public async Task<IUser> UpdateUserPasswordAsync(UserUpdatePasswordRequest request, long userId)
        {
            IHashProvider hashProvider = new HashProvider();

            var user = await _repository.GetUserByIdAsync(userId);

            if (user == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "User not found.");

            user.Password = hashProvider.ComputeHash(request.Password);

            await _repository.UpdateUserAsync(user);

            return user;
        }

        public async Task DeleteUserAsync(long userId)
        {
            var user = await _repository.GetUserByIdAsync(userId);

            if (user == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "User not found.");

            await _repository.DeleteUserAsync(userId);
        }
    }
}
