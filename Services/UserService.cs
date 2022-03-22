using DesafioStone.Interfaces.Repositories;
using DesafioStone.Interfaces.Services;
using DesafioStone.Models;
using DesafioStone.DataContracts;
using DesafioStone.Enums;
using DesafioStone.Utils.Common;
using System.Net;

namespace DesafioStone.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IHashService _hashService;
        private readonly IPasswordService _passwordService;

        public UserService(IUserRepository repository, IHashService hashService, IPasswordService passwordService)
        {
            _repository = repository;
            _hashService = hashService;
            _passwordService = passwordService;
        }

        public async Task<User> VerifyPasswordAsync(string username, string password)
        {
            var user = await _repository.GetUserByUsernameAsync(username);

            if (user == null && !_passwordService.IsValid(password, user.Password))
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "Invalid username or password.");

            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _repository.GetUserByUsernameAsync(username);

            if (user == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "User not found.");

            return user;
        }

        public async Task<User> GetUserByIdAsync(long userId)
        {
            var user = await _repository.GetUserByIdAsync(userId);

            if (user == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "User not found.");

            return user;
        }

        public async Task<User> CreateUserAsync(UserCreateRequest request)
        {
            var user = await _repository.GetUserByUsernameAsync(request.Username);

            if (user != null)
                throw Helpers.BuildHttpException(HttpStatusCode.Conflict, "Username already exists.");

            var insertedId = await _repository.InsertUserAsync(new User()
            {
                Username = request.Username,
                Password = _hashService.ComputeHash(request.Password),
                Role = request.Role ?? RoleEnum.User
            });

            return await _repository.GetUserByIdAsync(insertedId);
        }

        public async Task<User> UpdateUserPasswordAsync(UserUpdatePasswordRequest request, long userId)
        {
            var user = await _repository.GetUserByIdAsync(userId);

            if (user == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "User not found.");

            user.Password = _hashService.ComputeHash(request.Password);

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
