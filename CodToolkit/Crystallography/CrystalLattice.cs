using System;
using CodToolkit.Algebra;

namespace CodToolkit.Crystallography
{
    public class CrystalLattice
    {
        public ICrystalLatticeParameters CrystalLatticeParameters { get; }

        public ISpaceGroupInfo SpaceGroupInfo { get; }

        public IMatrix3X3 MetricTensor { get; }

        public CrystalLattice(ICrystalLatticeParameters crystalLatticeParameters, 
            ISpaceGroupInfo spaceGroupInfo)
        {
            CrystalLatticeParameters = crystalLatticeParameters;
            SpaceGroupInfo = spaceGroupInfo;
            MetricTensor = GetMetricTensor(crystalLatticeParameters);
        }

        private static IMatrix3X3 GetMetricTensor(ICrystalLatticeParameters parameters)
        {
            var constA = parameters.ConstA;
            var constB = parameters.ConstB;
            var constC = parameters.ConstC;
            var alpha = parameters.Alpha * Math.PI / 180;
            var beta = parameters.Beta * Math.PI / 180;
            var gamma = parameters.Gamma * Math.PI / 180;

            var metricTensor = new Matrix3X3
            {
                [0, 0] = constA * constA,
                [1, 1] = constB * constB,
                [2, 2] = constC * constC,
                [0, 1] = constA * constB * Math.Cos(gamma),
                [0, 2] = constA * constC * Math.Cos(beta),
                [1, 2] = constB * constC * Math.Cos(alpha)
            };
            metricTensor[1, 0] = metricTensor[0, 1];
            metricTensor[2, 0] = metricTensor[0, 2];
            metricTensor[2, 1] = metricTensor[1, 2];

            return metricTensor;
        }
    }
}
