namespace DesafioStone.Utils.Common
{
    public static class DBAccess
    {
        public static string ConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            var configuration = builder.Build();

            return configuration.GetValue<string>("ConnectionString");
        }
    }
}
