using DesafioStone.Models;
using DesafioStone.DataContracts;
using DesafioStone.Enums;
using DesafioStone.Utils.Providers.HashProvider;
using DesafioStone.Utils.Common;

namespace DesafioStone.Repositories
{
    public static class UserRepository
    {
        public static User VerifyPassword(string username, string password)
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "batman",
                    Password = "$s2$16384$8$1$BJ8NmuCKc2VJdjF9R/a90fHmiAqQ5ZG7oMv0RVxbBw4=$kviELeqMR0K/Z64N3NXm/5AFLMwV9e36g1MmZri0zQY=",
                    Role = Role.Manager
                }
            };

            var user = users.Where(x => x.Username.ToLower() == username.ToLower()).FirstOrDefault();

            if (Password.IsValid(password, user.Password))
            {
                return user;
            }

            return null;
        }

        public static User UsernameExists(string username)
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "batman",
                    Password = "$s2$16384$8$1$BJ8NmuCKc2VJdjF9R/a90fHmiAqQ5ZG7oMv0RVxbBw4=$kviELeqMR0K/Z64N3NXm/5AFLMwV9e36g1MmZri0zQY=",
                    Role = Role.Manager
                }
            };

            return users.Where(x => x.Username.ToLower() == username.ToLower()).FirstOrDefault();
        }

        public static User Create(UserCreateRequest request)
        {
            IHashProvider hashProvider = new HashProvider();

            var user = new User
            {
                Username = request.Username,
                Password = hashProvider.ComputeHash(request.Password),
                Role = request.Role
            };

            return user;
        }

        public static User GetUser(int id)
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "batman",
                    Password = "$s2$16384$8$1$BJ8NmuCKc2VJdjF9R/a90fHmiAqQ5ZG7oMv0RVxbBw4=$kviELeqMR0K/Z64N3NXm/5AFLMwV9e36g1MmZri0zQY=",
                    Role = Role.Manager
                }
            };

            return users.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
