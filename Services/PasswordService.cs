using DesafioStone.Interfaces.Services;

namespace DesafioStone.Services
{
    public class PasswordService : IPasswordService
    {
        public bool IsValid(string passwordRequest, string passwordBase)
        {
            var hashProvider = new HashService();

            if (hashProvider.CompareHash(passwordRequest, passwordBase))
            {
                return true;
            }

            return false;
        }
    }
}
