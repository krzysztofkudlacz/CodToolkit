using System;
using CodToolkit.Algebra;

namespace CodToolkit.Crystallography
{
    public class CrystalLattice
    {
        public ICrystalLatticeParameters CrystalLatticeParameters { get; }

        public ISpaceGroupInfo SpaceGroupInfo { get; }

        public IMatrix3X3 MetricTensor { get; }

        public IMatrix3X3 TransitionMatrix { get; }

        public CrystalLattice(ICrystalLatticeParameters crystalLatticeParameters, 
            ISpaceGroupInfo spaceGroupInfo)
        {
            CrystalLatticeParameters = crystalLatticeParameters;
            SpaceGroupInfo = spaceGroupInfo;
            TransitionMatrix = GetTransitionMatrix(crystalLatticeParameters, spaceGroupInfo);
            MetricTensor = Matrix3X3.Multiply(TransitionMatrix, TransitionMatrix.Transpose());
        }

        private static IMatrix3X3 GetTransitionMatrix(ICrystalLatticeParameters parameters, 
            ISpaceGroupInfo spaceGroupInfo)
        {
            var aConst = parameters.ConstA;
            var bConst = parameters.ConstB;
            var cConst = parameters.ConstC;

            var alpha = parameters.Alpha;
            var beta = parameters.Beta;
            var gamma = parameters.Gamma;

            if (spaceGroupInfo.Symbol.StartsWith("R"))
            {
                var cos = Math.Cos(120.0 * Math.PI / 180.0);
                var sin = Math.Sin(120.0 * Math.PI / 180.0);
                var cosAlpha = Math.Cos(alpha * Math.PI / 180.0);
                var a2 = Math.Pow(aConst, 2.0);
                var ax = Math.Sqrt(0.5 * a2 * (1.0 - cosAlpha) / Math.Pow(sin, 2.0));
                var az = Math.Sqrt(a2 - Math.Pow(ax, 2.0));

                var transitionMatrix = new Matrix3X3
                {
                    [0, 0] = ax,
                    [1, 0] = 0.0,
                    [2, 0] = az,
                    [0, 1] = cos * ax,
                    [1, 1] = sin * ax,
                    [2, 1] = az,
                    [0, 2] = cos * ax,
                    [1, 2] = -sin * ax,
                    [2, 2] = az
                };

                var rotation = RotationAroundZAxis(30.0 * Math.PI / 180.0);

                return Matrix3X3.Multiply(rotation, transitionMatrix);
            }
            else
            {
                var vecA = new double[3];
                var vecB = new double[3];
                var vecC = new double[3];

                vecC[0] = 0.0;
                vecC[1] = 0.0;
                vecC[2] = cConst;

                vecA[0] = aConst * Math.Sin(beta * Math.PI / 180.0);
                vecA[1] = 0.0;
                vecA[2] = aConst * Math.Cos(beta * Math.PI / 180.0);

                vecB[2] = bConst * Math.Cos(alpha * Math.PI / 180.0);
                vecB[0] = (aConst * bConst * Math.Cos(gamma * Math.PI / 180.0) - vecA[2] * vecB[2]) / vecA[0];
                vecB[1] = Math.Sqrt(Math.Pow(bConst, 2.0) - (Math.Pow(vecB[0], 2.0) + Math.Pow(vecB[2], 2.0)));

                var transitionMatrix = new Matrix3X3();

                for (var i = 0; i < 3; i++)
                {
                    transitionMatrix[i, 0] = vecA[i];
                    transitionMatrix[i, 1] = vecB[i];
                    transitionMatrix[i, 2] = vecC[i];
                }

                return transitionMatrix;
            }
        }

        private static IMatrix3X3 RotationAroundZAxis(double angle)
        {
            return new Matrix3X3
            {
                [0, 0] = Math.Cos(angle),
                [0, 1] = -1.0 * Math.Sin(angle),
                [0, 2] = 0.0,
                [1, 0] = Math.Sin(angle),
                [1, 1] = Math.Cos(angle),
                [1, 2] = 0.0,
                [2, 0] = 0.0,
                [2, 1] = 0.0,
                [2, 2] = 1.0
            };
        }
    }
}
