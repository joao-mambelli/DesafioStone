using Autofac.Extras.Moq;
using DesafioStone.Models;
using DesafioStone.Enums;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using DesafioStone.Services;
using DesafioStone.Interfaces.Providers;

namespace DesafioStone.UnitTests.Services
{
    [TestClass]
    public class TokenServiceTests
    {
        [TestMethod]
        public void CanGenerateToken_ReturnsString()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<JwtSecurityTokenHandler>().Setup(x => x.CreateToken(It.IsAny<SecurityTokenDescriptor>())).Returns(new JwtSecurityToken()).Verifiable();
            mock.Mock<JwtSecurityTokenHandler>().Setup(x => x.WriteToken(It.IsAny<SecurityToken>())).Returns("test").Verifiable();
            mock.Mock<ISecretProvider>().Setup(x => x.Secret()).Returns("secret").Verifiable();

            var service = mock.Create<TokenService>();

            var user = new User
            {
                Username = "",
                Role = RoleEnum.User
            };

            var result = service.GenerateToken(user);

            Assert.IsTrue(result == "test");
        }
    }
}
