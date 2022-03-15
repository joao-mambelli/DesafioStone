using DesafioStone.Models;

namespace DesafioStone.Repositories
{
    public static class UserRepository
    {
        public static User? Get(string? username, string? password)
        {
            if (username == null || password == null)
                return null;

            var users = new List<User>();

            users.Add(new User
            {
                Id = 1,
                Username = "batman",
                Password = "batman"
            });

            return users.Where(x => x.Username?.ToLower() == username.ToLower() && x.Password == password).FirstOrDefault();
        }
    }
}
