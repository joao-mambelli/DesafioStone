using Autofac.Extras.Moq;
using DesafioStone.Models;
using DesafioStone.Enums;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using DesafioStone.Providers;

namespace DesafioStone.UnitTests.Providers
{
    [TestClass]
    public class TokenProviderTests
    {
        [TestMethod]
        public void CanGenerateToken_ReturnsString()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<JwtSecurityTokenHandler>().Setup(x => x.CreateToken(It.IsAny<SecurityTokenDescriptor>())).Returns(new JwtSecurityToken()).Verifiable();
            mock.Mock<JwtSecurityTokenHandler>().Setup(x => x.WriteToken(It.IsAny<SecurityToken>())).Returns("test").Verifiable();

            var provider = mock.Create<TokenProvider>();

            var user = new User
            {
                Username = "",
                Role = RoleEnum.User
            };

            var result = provider.GenerateToken(user);

            Assert.IsTrue(result == "test");
        }
    }
}
