using DesafioStone.Enums;

namespace DesafioStone.DataContracts
{
    public class UserCreateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}