namespace DesafioStone.Utils.Common
{
    public static class AccessDataBase
    {
        public static string ConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            var configuration = builder.Build();

            var connString = configuration.GetValue<string>("ConnectionString");

            if (connString == null)
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
