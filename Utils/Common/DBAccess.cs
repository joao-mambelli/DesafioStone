﻿namespace DesafioStone.Utils.Common
{
    public static class DBAccess
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

            return connString;
        }
    }
}