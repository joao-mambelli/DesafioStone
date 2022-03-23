using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesafioStone.Factories;
using DesafioStone.Interfaces.Providers;

namespace DesafioStone.UnitTests.Factories
{
    [TestClass]
    public class MySqlConnectionFactoryTests
    {
        [TestMethod]
        public void CanCreateConnection_ReturnsMySqlConnection()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IConnectionStringProvider>().Setup(x => x.ConnectionString()).Returns("Data Source=0.0.0.0,0;Initial Catalog=desafiostone;User ID=api;Password=123").Verifiable();

            var factory = mock.Create<MySqlConnectionFactory>();

            var result = factory.CreateConnection();

            Assert.IsTrue(result != null);
        }
    }
}
