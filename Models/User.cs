using DesafioStone.Enums;

namespace DesafioStone.Models
{
    public class User
    {
        internal int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}