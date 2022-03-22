using DesafioStone.Interfaces.Services;

namespace DesafioStone.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IHashService _hashService;

        public PasswordService(IHashService hashService)
        {
            _hashService = hashService;
        }

        public bool IsValid(string passwordRequest, string passwordBase)
        {
            if (_hashService.CompareHash(passwordRequest, passwordBase))
            {
                return true;
            }

            return false;
        }
    }
}
