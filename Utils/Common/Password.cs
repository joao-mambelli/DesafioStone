using DesafioStone.Utils.Providers.HashProvider;

namespace DesafioStone.Utils.Common
{
    public static class Password
    {
        public static bool IsValid(string passwordRequest, string passwordBase)
        {
            IHashProvider hashProvider = new HashProvider();

            if (hashProvider.CompareHash(passwordRequest, passwordBase))
            {
                return true;
            }

            return false;
        }
    }
}
