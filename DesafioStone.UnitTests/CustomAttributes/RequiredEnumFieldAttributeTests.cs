using Autofac.Extras.Moq;
using DesafioStone.CustomAttributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesafioStone.Enums;

namespace DesafioStone.UnitTests.CustomAttributes
{
    [TestClass]
    public class RequiredEnumFieldAttributeTests
    {
        [TestMethod]
        public void CanValidate_ValidEnum_ReturnsTrue()
        {
            using var mock = AutoMock.GetLoose();

            var attribute = mock.Create<RequiredEnumFieldAttribute>();

            var result = attribute.IsValid(MonthEnum.January);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanValidate_InvalidEnum_ReturnsFalse()
        {
            using var mock = AutoMock.GetLoose();

            var attribute = mock.Create<RequiredEnumFieldAttribute>();

            var result = attribute.IsValid((MonthEnum) 0);

            Assert.IsTrue(!result);
        }
    }
}
