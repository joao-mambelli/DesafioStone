using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesafioStone.Providers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

namespace DesafioStone.UnitTests.Providers
{
    [TestClass]
    public class SecretProviderTests
    {
        [TestMethod]
        public void CanGetSecret_InAppSettings_ReturnsString()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {
                    "Secret", "secret"
                }
            };

            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

            using var mock = AutoMock.GetLoose();
            mock.Mock<IConfigurationBuilder>().Setup(x => x.Build()).Returns((IConfigurationRoot)configuration).Verifiable();

            var provider = mock.Create<SecretProvider>();

            var result = provider.Secret();

            Assert.IsTrue(result == "secret");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CanGetSecret_NoSecret_ReturnsString()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                
            };

            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

            using var mock = AutoMock.GetLoose();
            mock.Mock<IConfigurationBuilder>().Setup(x => x.Build()).Returns((IConfigurationRoot)configuration).Verifiable();

            var provider = mock.Create<SecretProvider>();

            var result = provider.Secret();
        }
    }
}
