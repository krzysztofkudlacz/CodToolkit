using System;

namespace CodToolkit.Algebra
{
    public interface IVector3
    {
        double this[int index] { get; }

        double Norm { get; }
    }

    public class Vector3 : IVector3
    {
        private readonly double[] _vector;

        public Vector3()
        {
            _vector= new double[3];
        }

        public double this[int index]
        {
            get => _vector[index];
            set => _vector[index] = value;
        }

        public static double ScalarProduct(
            IVector3 vector1, 
            IVector3 vector2)
        {
            var sp = 0.0;

            for (var i = 0; i < 3; i++)
                sp += vector1[i] * vector2[i];

            return sp;
        }

        public static IVector3 VectorProduct(
            IVector3 vector1, 
            IVector3 vector2)
        {
            var vector = new Vector3
            {
                [0] = vector1[1] * vector2[2] - vector1[2] * vector2[1],
                [1] = vector1[2] * vector2[0] - vector1[0] * vector2[2],
                [2] = vector1[0] * vector2[1] - vector1[1] * vector2[0]
            };

            return vector;
        }

        public double Norm => Math.Sqrt(
            Math.Pow(this[0], 2) + 
            Math.Pow(this[1], 2) + 
            Math.Pow(this[2], 2));
    }
}
