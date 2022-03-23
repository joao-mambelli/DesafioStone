using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesafioStone.Providers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

namespace DesafioStone.UnitTests.Providers
{
    [TestClass]
    public class ConnectionStringProviderTests
    {
        [TestMethod]
        public void CanGetConnectionString_InAppSettings_ReturnsString()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {
                    "ConnectionString", "connectionString"
                }
            };

            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

            using var mock = AutoMock.GetLoose();
            mock.Mock<IConfigurationBuilder>().Setup(x => x.Build()).Returns((IConfigurationRoot)configuration).Verifiable();

            var provider = mock.Create<ConnectionStringProvider>();

            var result = provider.ConnectionString();

            Assert.IsTrue(result == "connectionString");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CanGetConnectionString_NoSecret_ReturnsString()
        {
            var inMemorySettings = new Dictionary<string, string>
            {

            };

            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

            using var mock = AutoMock.GetLoose();
            mock.Mock<IConfigurationBuilder>().Setup(x => x.Build()).Returns((IConfigurationRoot)configuration).Verifiable();

            var provider = mock.Create<ConnectionStringProvider>();

            var result = provider.ConnectionString();
        }
    }
}
