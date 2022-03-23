using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesafioStone.Factories;

namespace DesafioStone.UnitTests.Factories
{
    [TestClass]
    public class MySqlConnectionFactoryTests
    {
        [TestMethod]
        public void CanCreateConnection_ReturnsMySqlConnection()
        {
            using var mock = AutoMock.GetLoose();

            var factory = mock.Create<MySqlConnectionFactory>();

            var result = factory.CreateConnection();

            Assert.IsTrue(result != null);
        }
    }
}
