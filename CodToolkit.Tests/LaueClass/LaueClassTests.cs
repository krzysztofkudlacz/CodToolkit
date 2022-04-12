using CodToolkit.LaueClass;
using NUnit.Framework;

namespace CodToolkit.Tests.LaueClass
{
    [TestFixture]
    public class LaueClassTests
    {
        [TestCase("R 3 2:R", "-31m")]
        [TestCase("R 3 2:H", "-31m")]
        [TestCase("R -3 : H", "-3")]
        [TestCase("R -3 c :H", "-3m")]
        public void When_Called_Should_ReturnProperLaueClass(string hallName, string lauClassSymbol)
        {
            Assert.AreEqual(
                SpaceGroupToLaueClassMapper
                    .LaueClassSymbol(hallName), 
                lauClassSymbol);
        }
    }
}
