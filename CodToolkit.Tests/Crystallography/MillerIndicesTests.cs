using CodToolkit.Crystallography;
using NUnit.Framework;

namespace CodToolkit.Tests.Crystallography
{
    [TestFixture]
    public class MillerIndicesTests
    {
        [TestCase(1, 1, 1, -1, -1, -1, true, true)]
        [TestCase(1, 1, 1, -1, -1, -1, false, false)]
        [TestCase(1, 1, 1, 1, 1, 1, false, true)]
        public void When_Compared_Should_GiveAssumedResult(
            int h1, int k1, int l1,
            int h2, int k2, int l2,
            bool asFriedelPair,
            bool areEqual)
        {
            Assert.True(
                new MillerIndices(h1, k1, l1)
                    .AreEqual(new MillerIndices(h2, k2, l2), 
                        asFriedelPair) == areEqual);
        }
    }
}
