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

        public User VerifyPassword(string username, string password)
        {
            var user = _repository.GetUserByUsername(username);

            if (user == null || !_passwordService.IsValid(password, user.Password))
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "Invalid username or password.");

            return user;
        }

        public User GetUserByUsername(string username)
        {
            var user = _repository.GetUserByUsername(username);

            if (user == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "User not found.");

            return user;
        }

        public User GetUserById(long userId)
        {
            var user = _repository.GetUserById(userId);

            if (user == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "User not found.");

            return user;
        }

        public User CreateUser(UserCreateRequest request)
        {
            var user = _repository.GetUserByUsername(request.Username);

            if (user != null)
                throw Helpers.BuildHttpException(HttpStatusCode.Conflict, "Username already exists.");

            var insertedId = _repository.InsertUser(new User()
            {
                Username = request.Username,
                Password = _hashService.ComputeHash(request.Password),
                Role = request.Role ?? RoleEnum.User
            });

            return _repository.GetUserById(insertedId);
        }

        public User UpdateUserPassword(UserUpdatePasswordRequest request, long userId)
        {
            var user = _repository.GetUserById(userId);

            if (user == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "User not found.");

            user.Password = _hashService.ComputeHash(request.Password);

            _repository.UpdateUser(user);

            return user;
        }

        public void DeleteUser(long userId)
        {
            var user = _repository.GetUserById(userId);

            if (user == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "User not found.");

            _repository.DeleteUser(userId);
        }

        public void LogoutAllDevices(long userId)
        {
            var user = _repository.GetUserById(userId);

            if (user == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "User not found.");

            user.LastLogoutAllRequest = DateTime.UtcNow;

            _repository.UpdateUser(user);
        }
    }
}
