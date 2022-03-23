namespace DesafioStone.Interfaces.Services
{
    public interface IPasswordService
    {
        bool IsValid(string passwordRequest, string passwordBase);
    }
}
