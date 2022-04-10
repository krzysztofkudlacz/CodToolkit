using System;

namespace CodToolkit.Algebra
{
    public interface IMatrix3X3
    {
        double this[int row, int column] { get; }

        IMatrix3X3 Inverse();
    }

    public class Matrix3X3 : IMatrix3X3
    {
        private readonly double[,] _matrix;

        public Matrix3X3()
        {
            _matrix = new double[3, 3];
        }

        private Matrix3X3(double[,] matrix)
        {
            _matrix = matrix;
        }

        public double this[int row, int column]
        {
            get => _matrix[row, column];
            set => _matrix[row, column] = value;
        }

        public static IMatrix3X3 Multiply(Matrix3X3 matrix1, Matrix3X3 matrix2)
        {
            var matrix = new Matrix3X3();

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    for (var k = 0; k < 3; k++)
                        matrix[i, j] += matrix1[i, k] * matrix2[k, j];
                }
            }

            return matrix;
        }

        public bool IsEqual(IMatrix3X3 matrix, double precision)
        {
            return AreEqual(this, matrix, precision);
        }

        public static bool AreEqual(IMatrix3X3 matrix1, IMatrix3X3 matrix2, double precision)
        {
            var areEqual = true;
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                    areEqual = areEqual && Math.Abs(matrix1[i, j] - matrix2[i, j]) < precision;
            }

            return areEqual;
        }

        public IMatrix3X3 Inverse()
        {
            var matrixToInverse = new double[3, 3];

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                    matrixToInverse[i, j] = this[i, j];
            }

            return new Matrix3X3(InverseMatrix(matrixToInverse));
        }

        private static double[,] InverseMatrix(double[,] matrix)
        {
            var order = matrix.GetLength(0);
            var determinant = MatrixDeterminant(matrix);
            var transposition = TransposeMatrix(MatrixAdjoint(matrix));
            var inversion = new double[order, order];

            for (var i = 0; i < order; i++)
            {
                for (var j = 0; j < order; j++)
                    inversion[i, j] = transposition[i, j] / determinant;
            }

            return inversion;
        }

        private static double MatrixDeterminant(double[,] matrix)
        {
            var order = matrix.GetLength(0);
            if (order == 1) { return matrix[0, 0]; }

            var determinant = 0.0;
            for (var i = 0; i < order; i++)
            {
                determinant += matrix[0, i] * MatrixDeterminant(MatrixMinor(matrix, 0, i)) *
                               Math.Pow(-1.0, i);
            }
            return determinant;
        }

        public static double[,] TransposeMatrix(double[,] matrix)
        {
            var rowCount = matrix.GetLength(0);
            var columnCount = matrix.GetLength(1);

            var transposition = new double[columnCount, rowCount];

            for (var i = 0; i < rowCount; i++)
            {
                for (var j = 0; j < columnCount; j++)
                    transposition[j, i] = matrix[i, j];
            }

            return transposition;
        }

        private static double[,] MatrixAdjoint(double[,] matrix)
        {
            var order = matrix.GetLength(0);
            var adjoint = new double[order, order];

            for (var i = 0; i < order; i++)
            {
                for (var j = 0; j < order; j++)
                {
                    adjoint[i, j] = Math.Pow(-1.0, i + j) * 
                                    MatrixDeterminant(MatrixMinor(matrix, i, j));
                }
            }

            return adjoint;
        }

        private static double[,] MatrixMinor(double[,] matrix, int row, int column)
        {
            var order = matrix.GetLength(0);
            var minor = new double[order - 1, order - 1];
            var r = 0;

            for (var i = 0; i < order; i++)
            {
                if (i == row) continue;
                var c = 0;
                for (var j = 0; j < order; j++)
                {
                    if (j == column) continue;
                    minor[r, c] = matrix[i, j];
                    c++;
                }
                r++;
            }
            return minor;
        }
    }
}
