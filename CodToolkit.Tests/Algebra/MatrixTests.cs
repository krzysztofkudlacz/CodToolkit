using CodToolkit.Algebra;
using NUnit.Framework;

namespace CodToolkit.Tests.Algebra
{
    [TestFixture]
    public class MatrixTests
    {
        [Test]
        public void When_Inverted_Should_BeEqualToExpectedMatrix()
        {
            const double precision = 1e-5;
            var matrix = new Matrix3X3
            {
                [0, 0] = 1.0,
                [0, 1] = 2.0,
                [0, 2] = 3.0,
                [1, 0] = 0.0,
                [1, 1] = 1.0,
                [1, 2] = 4.0,
                [2, 0] = 5.0,
                [2, 1] = 6.0,
                [2, 2] = 0.0
            };

            var expected = new Matrix3X3
            {
                [0, 0] = -24,
                [0, 1] = 18,
                [0, 2] = 5,
                [1, 0] = 20,
                [1, 1] = -15,
                [1, 2] = -4,
                [2, 0] = -5,
                [2, 1] = 4,
                [2, 2] = 1
            };

            var inversion = matrix.Inverse();

            Assert.True(Matrix3X3.AreEqual(inversion, expected, precision));
        }

        [Test]
        public void When_Multiplied_Should_BeEqualToExpectedMatrix()
        {
            const double precision = 1e-5;
            var matrix1 = new Matrix3X3
            {
                [0, 0] = 1.0,
                [0, 1] = 2.0,
                [0, 2] = 3.0,
                [1, 0] = 4.0,
                [1, 1] = 5.0,
                [1, 2] = 6.0,
                [2, 0] = 7.0,
                [2, 1] = 8.0,
                [2, 2] = 9.0
            };

            var matrix2 = new Matrix3X3
            {
                [0, 0] = 1.0,
                [0, 1] = 4.0,
                [0, 2] = 7.0,
                [1, 0] = 2.0,
                [1, 1] = 5.0,
                [1, 2] = 8.0,
                [2, 0] = 3.0,
                [2, 1] = 6.0,
                [2, 2] = 9.0
            };

            var expected = new Matrix3X3
            {
                [0, 0] = 14,
                [0, 1] = 32,
                [0, 2] = 50,
                [1, 0] = 32,
                [1, 1] = 77,
                [1, 2] = 122,
                [2, 0] = 50,
                [2, 1] = 122,
                [2, 2] = 194
            };

            var matrix = Matrix3X3.Multiply(matrix1, matrix2);

            Assert.True(Matrix3X3.AreEqual(matrix, expected, precision));
        }
    }
}
