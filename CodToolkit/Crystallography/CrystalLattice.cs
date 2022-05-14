using System;
using System.Collections.Generic;
using System.Linq;
using CodToolkit.Algebra;
using CodToolkit.LaueClass;

namespace CodToolkit.Crystallography
{
    public class CrystalLattice
    {
        public ICrystalLatticeParameters CrystalLatticeParameters { get; }

        public IVector3 VecA { get; }

        public IVector3 VecB { get; }

        public IVector3 VecC { get; }

        public IVector3 RecVecA { get; }

        public IVector3 RecVecB { get; }

        public IVector3 RecVecC { get; }

        public IMatrix3X3 MetricTensor { get; }

        public IMatrix3X3 RecMetricTensor { get; }

        public ILaueClass LaueClass { get; }

        public CrystalLattice(
            ICrystalLatticeParameters crystalLatticeParameters,
            string[] spaceGroupSymbolCandidates)
        {
            CrystalLatticeParameters = crystalLatticeParameters;

            var (vecA, vecB, vecC) = GetBase(
                CrystalLatticeParameters, 
                spaceGroupSymbolCandidates);

            VecA = vecA;
            VecB = vecB;
            VecC = vecC;

            var (recVecA, recVecB, recVecC) = GetReciprocalBase(this);

            RecVecA = recVecA;
            RecVecB = recVecB;
            RecVecC = recVecC;

            MetricTensor = GetMetricTensor(crystalLatticeParameters);

            RecMetricTensor = GetReciprocalMetricTensor(this);

            LaueClass = LaueClassCreator
                .CreateLaueClass(
                    SpaceGroupToLaueClassMapper
                        .LaueClassSymbol(spaceGroupSymbolCandidates));
        }

        private static (IVector3 VecA, IVector3 VecB, IVector3 VecC) GetBase(
            ICrystalLatticeParameters parameters,
            IEnumerable<string> spaceGroupSymbolCandidates)
        {
            IVector3 vecA, vecB, vecC;

            var aConst = parameters.ConstA;
            var bConst = parameters.ConstB;
            var cConst = parameters.ConstC;

            var alpha = parameters.Alpha;
            var beta = parameters.Beta;
            var gamma = parameters.Gamma;

            if (spaceGroupSymbolCandidates
                .Any(
                    c => c
                        .ToUpper()
                        .Contains(":R")))
            {
                var cos = Math.Cos(120.0 * Math.PI / 180.0);
                var sin = Math.Sin(120.0 * Math.PI / 180.0);
                var cosAlpha = Math.Cos(alpha * Math.PI / 180.0);
                var a2 = Math.Pow(aConst, 2.0);
                var ax = Math.Sqrt(0.5 * a2 * (1.0 - cosAlpha) / Math.Pow(sin, 2.0));
                var az = Math.Sqrt(a2 - Math.Pow(ax, 2.0));

                var av = new Vector3 {[0] = ax, [1] = 0.0, [2] = az};
                var bv = new Vector3 {[0] = cos * ax, [1] = sin * ax, [2] = az};
                var cv = new Vector3 {[0] = cos * ax, [1] = -sin * ax, [2] = az};

                var rotation = RotationAroundZAxis(30.0 * Math.PI / 180.0);

                vecA = Matrix3X3.Multiply(rotation, av);
                vecB = Matrix3X3.Multiply(rotation, bv);
                vecC = Matrix3X3.Multiply(rotation, cv);
            }
            else
            {
                vecC = new Vector3
                {
                    [0] = 0.0,
                    [1] = 0.0,
                    [2] = cConst,
                };

                vecA = new Vector3
                {
                    [0] = aConst * Math.Sin(beta * Math.PI / 180.0),
                    [1] = 0.0,
                    [2] = aConst * Math.Cos(beta * Math.PI / 180.0)
                };

                var b2 = bConst * Math.Cos(alpha * Math.PI / 180.0);
                var b0 = (aConst * bConst * Math.Cos(gamma * Math.PI / 180.0) - vecA[2] * b2) / vecA[0];
                var b1 = Math.Sqrt(Math.Pow(bConst, 2.0) - (Math.Pow(b0, 2.0) + Math.Pow(b2, 2.0)));

                vecB = new Vector3
                {
                    [0] = b0,
                    [1] = b1,
                    [2] = b2
                };
            }

            return (
                VecA: vecA, 
                VecB: vecB, 
                VecC: vecC);
        }

        private static (IVector3 RecVecA, IVector3 RecVecB, IVector3 RecVecC) GetReciprocalBase(
            CrystalLattice crystalLattice)
        {
            var vecA = crystalLattice.VecA;
            var vecB = crystalLattice.VecB;
            var vecC = crystalLattice.VecC;

            var vp = Vector3.VectorProduct(vecA, vecB);
            var vol = Vector3.ScalarProduct(vp, vecC);

            var recVecA = Vector3.VectorProduct(vecB, vecC);
            var recVecB = Vector3.VectorProduct(vecC, vecA);
            var recVecC = Vector3.VectorProduct(vecA, vecB);

            var recVecANormalized = new Vector3();
            var recVecBNormalized = new Vector3();
            var recVecCNormalized = new Vector3();

            for (var i = 0; i < 3; i++)
            {
                recVecANormalized[i] = recVecA[i] / vol;
                recVecBNormalized[i] = recVecB[i] / vol;
                recVecCNormalized[i] = recVecC[i] / vol;
            }

            return (
                RecVecA: recVecANormalized, 
                RecVecB: recVecBNormalized, 
                RecVecC: recVecCNormalized);
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

            return metricTensor.Transpose();
        }

        public static IMatrix3X3 GetReciprocalMetricTensor(CrystalLattice crystalLattice)
        {
            var recVecA = crystalLattice.RecVecA;
            var recVecB = crystalLattice.RecVecB;
            var recVecC = crystalLattice.RecVecC;

            var recMetricTensor = new Matrix3X3
            {
                [0, 0] = Vector3.ScalarProduct(recVecA, recVecA),
                [1, 1] = Vector3.ScalarProduct(recVecB, recVecB),
                [2, 2] = Vector3.ScalarProduct(recVecC, recVecC),
                [0, 1] = Vector3.ScalarProduct(recVecA, recVecB),
                [0, 2] = Vector3.ScalarProduct(recVecA, recVecC),
                [1, 2] = Vector3.ScalarProduct(recVecB, recVecC)
            };
            recMetricTensor[1, 0] = recMetricTensor[0, 1];
            recMetricTensor[2, 0] = recMetricTensor[0, 2];
            recMetricTensor[2, 1] = recMetricTensor[1, 2];

            return recMetricTensor.Transpose();
        }

        private static IMatrix3X3 RotationAroundZAxis(
            double angle)
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

        public IMillerIndices[] SymmetricalMillerIndices(
            IMillerIndices millerIndices)
        {
            var metricTensor = MetricTensor;
            var recMetricTensor = RecMetricTensor;

            var hkl = new Vector3
            {
                [0] = millerIndices.H, 
                [1] = millerIndices.K, 
                [2] = millerIndices.L
            };

            var hklVector = Matrix3X3.Multiply(recMetricTensor, hkl);

            var symEqHklList = new List<IMillerIndices>();

            foreach (var group in LaueClass)
            {
                var vector = Matrix3X3.Multiply(group, hklVector);
                var eqHkl = Matrix3X3.Multiply(metricTensor, vector);

                var h = (int)Math.Round(eqHkl[0], 0);
                var k = (int)Math.Round(eqHkl[1], 0);
                var l = (int)Math.Round(eqHkl[2], 0);

                var eqMi = new MillerIndices(h, k, l);

                if (symEqHklList.Exists(m => m.AreEqual(eqMi, true)))
                    continue;

                symEqHklList.Add(eqMi);
            }

            return symEqHklList.ToArray();
        }

        public IVector3 GetHklVector(IMillerIndices millerIndices)
        {
            var ra = RecVecA;
            var rb = RecVecB;
            var rc = RecVecC;

            var h = millerIndices.H;
            var k = millerIndices.K;
            var l = millerIndices.L;

            var vector = new Vector3
            {
                [0] = h * ra[0] + k * rb[0] + l * rc[0],
                [1] = h * ra[1] + k * rb[1] + l * rc[1],
                [2] = h * ra[2] + k * rb[2] + l * rc[2]
            };

            return vector;
        }
    }
}
