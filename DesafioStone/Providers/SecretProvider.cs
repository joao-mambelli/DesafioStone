using DesafioStone.Interfaces.Providers;

namespace DesafioStone.Providers
{
    public class SecretProvider : ISecretProvider
    {
        private readonly IConfigurationBuilder _configurationBuilder;

        public SecretProvider(IConfigurationBuilder configurationBuilder)
        {
            _configurationBuilder = configurationBuilder;
        }

        public string Secret()
        {
            var builder = _configurationBuilder;

            if (builder.Properties != null && builder.Sources != null)
            {
                builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            }

            var configuration = builder.Build();

            var secret = configuration.GetValue<string>("Secret");

            if (secret == null && builder.Properties != null && builder.Sources != null)
            {
                secret = Environment.GetEnvironmentVariable("DesafioStoneSecret");
            }

            if (secret == null)
            {
                throw new Exception("Secret string not found.");
            }

            return secret;
        }
    }
}
