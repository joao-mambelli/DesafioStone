using DesafioStone.Enums;
using DesafioStone.Interfaces;

namespace DesafioStone.Models
{
    public class User : IUser
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}