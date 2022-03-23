namespace DesafioStone.Utils.Common
{
    public static class AccessSecret
    {
        public static string Secret()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            var configuration = builder.Build();

            var secret = configuration.GetValue<string>("Secret");

            if (secret == null)
            {
                secret = Environment.GetEnvironmentVariable("DesafioStoneSecret");
            }

            return secret;
        }
    }
}
