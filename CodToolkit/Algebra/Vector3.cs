namespace CodToolkit.Algebra
{
    public interface IVector3
    {
        double this[int index] { get; }
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

        public static Vector3 ScalarProduct(
            Vector3 vector1, 
            Vector3 vector2)
        {
            var vector = new Vector3();

            for (var i = 0; i < 3; i++)
                vector[i] = vector1[i] * vector2[i];

            return vector;
        }

        public static Vector3 VectorProduct(
            Vector3 vector1, 
            Vector3 vector2)
        {
            var vector = new Vector3
            {
                [0] = vector1[1] * vector2[2] - vector1[2] * vector2[1],
                [1] = vector1[2] * vector2[0] - vector1[0] * vector2[2],
                [2] = vector1[0] * vector2[1] - vector1[1] * vector2[0]
            };

            return vector;
        }
    }
}
