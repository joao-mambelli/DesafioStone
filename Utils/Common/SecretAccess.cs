namespace DesafioStone.Utils.Common
{
    public static class SecretAccess
    {
        public static string Secret()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            var configuration = builder.Build();

            var connString = configuration.GetValue<string>("Secret");

            if (connString == null)
            {
                connString = Environment.GetEnvironmentVariable("DesafioStoneSecret");
            }

            return connString;
        }
    }
}
