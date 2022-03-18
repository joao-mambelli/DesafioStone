using DesafioStone.Enums;

namespace DesafioStone.Interfaces.ModelsInterfaces
{
    public interface IUser
    {
        long Id { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        RoleEnum Role { get; set; }
    }
}