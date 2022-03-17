using DesafioStone.Enums;

namespace DesafioStone.Interfaces
{
    public interface IUser
    {
        long Id { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        Role Role { get; set; }
    }
}