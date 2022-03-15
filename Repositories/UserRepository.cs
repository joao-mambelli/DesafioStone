using DesafioStone.Models;
using DesafioStone.Enums;

namespace DesafioStone.Repositories
{
    public static class UserRepository
    {
        public static User Get(string username, string password)
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "batman",
                    Password = "batman",
                    Role = Role.Manager
                }
            };

            return users.Where(x => x.Username?.ToLower() == username.ToLower() && x.Password == password).FirstOrDefault();
        }
    }
}
