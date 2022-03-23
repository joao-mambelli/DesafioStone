using DesafioStone.Interfaces.Providers;

namespace DesafioStone.Providers
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfigurationBuilder _configurationBuilder;

        public ConnectionStringProvider(IConfigurationBuilder configurationBuilder)
        {
            _configurationBuilder = configurationBuilder;
        }

        public string ConnectionString()
        {
            var builder = _configurationBuilder;

            if (builder.Properties != null && builder.Sources != null)
            {
                builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            }

            var configuration = builder.Build();

            var connString = configuration.GetValue<string>("ConnectionString");

            if (connString == null && builder.Properties != null && builder.Sources != null)
            {
                connString = Environment.GetEnvironmentVariable("DesafioStoneConnectionString");
            }

            if (connString == null)
            {
                throw new Exception("Connection string not found.");
            }

            return connString;
        }
    }
}
