namespace DesafioStone.Interfaces.Services
{
    public interface IHashService
    {
        bool CompareHash(string plainValue, string hashedValue);
        string ComputeHash(string value);
        bool ValidateHash(string hashedValue);
    }
}
