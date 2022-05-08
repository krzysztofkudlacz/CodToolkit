using System.Collections.Generic;
using System.Linq;
using CodToolkit.Cod;
using NUnit.Framework;

namespace CodToolkit.Tests.Cod
{
    [TestFixture]
    public class CodSearchParametersTests
    {
        [Test]
        public void When_Cloned_ShouldHaveTheSameStateAsSource()
        {
            var parameters = new CodSearchParameters
            {
                Text = "Text",
                RequiredElements = "RequiredElements",
                ExcludedElements = "ExcludedElements",
                MinNumberOfDistinctElements = "MinNumberOfDistinctElements",
                MaxNumberOfDistinctElements = "MaxNumberOfDistinctElements",
                MinA = "MinA",
                MaxA = "MaxA",
                MinB = "MinB",
                MaxB = "MaxB",
                MinC = "MinC",
                MinAlpha = "MinAlpha",
                MaxAlpha = "MaxAlpha",
                MinBeta = "MinBeta",
                MaxBeta = "MaxBeta",
                MinGamma = "MinGamma",
                MaxGamma = "MaxGamma"
            };

            var clone = parameters.Clone();

            var equalities = new List<bool>
            {
                parameters.Text == clone.Text,
                parameters.RequiredElements == clone.RequiredElements,
                parameters.ExcludedElements == clone.ExcludedElements,
                parameters.MinNumberOfDistinctElements == clone.MinNumberOfDistinctElements,
                parameters.MaxNumberOfDistinctElements == clone.MaxNumberOfDistinctElements,
                parameters.MinA == clone.MinA,
                parameters.MaxA == clone.MaxA,
                parameters.MinB == clone.MinB,
                parameters.MaxB == clone.MaxB,
                parameters.MinC == clone.MinC,
                parameters.MinAlpha == clone.MinAlpha,
                parameters.MaxAlpha == clone.MaxAlpha,
                parameters.MinBeta == clone.MinBeta,
                parameters.MaxBeta == clone.MaxBeta,
                parameters.MinGamma == clone.MinGamma,
                parameters.MaxGamma == clone.MaxGamma
            };
            
            Assert.True(equalities.All(e => e));
        }
    }
}
