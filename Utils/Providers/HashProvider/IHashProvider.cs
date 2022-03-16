namespace DesafioStone.Utils.Providers.HashProvider
{
    public interface IHashProvider
    {
        bool CompareHash(string plainValue, string hashedValue);
        string ComputeHash(string value);
        bool ValidateHash(string hashedValue);
    }
}
