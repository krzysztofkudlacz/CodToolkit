using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodToolkit.Crystallography;
using CodToolkit.Extensions;
using CodToolkit.XrdProfile;

namespace CodToolkit.Xrd
{
    public interface IXrdProfile
    {
        (double Q, double I, IMillerIndices Hkl)[] Peaks { get; }

        (double[] Q, double[] I) Profile { get; }
    }

    public class XrdProfile : IXrdProfile
    {
        public (double Q, double I, IMillerIndices Hkl)[] Peaks { get; set; }

        public (double[] Q, double[] I) Profile { get; set; }
    }

    public static class XrdCalculations
    {
        private const double MinQ = 0.0;
        private const double MaxQ = 2.5;
        private const int NumberOfPoints = 512;
        private const double PeakWidth = 5;
        private const double IntensityThreshold = 0.005;

        private static IReadOnlyCollection<MillerIndices> _millerIndices;

        private static IReadOnlyCollection<MillerIndices> MillerIndices => _millerIndices ??= GetMillerIndices();

        private static IReadOnlyCollection<MillerIndices> GetMillerIndices()
        {
            const int maxHkl = 5;
            const int hklCount = 2 * maxHkl + 1;

            var hRange = Enumerable.Range(-maxHkl, hklCount)
                .Reverse()
                .ToList();

            var kRange = Enumerable.Range(-maxHkl, hklCount)
                .Reverse()
                .ToList();

            var lRange = Enumerable.Range(-maxHkl, hklCount)
                .Reverse()
                .ToList();

            var millerIndicesCollection = hRange
                .CartesianProduct(
                    kRange,
                    (e1, e2) => (h: e1, k: e2))
                .CartesianProduct(
                    lRange, (t, l) => new MillerIndices(t.h, t.k, l));

            var millerIndicesWithoutFriedelPairs = new List<MillerIndices>();
            var millerIndices0 = new MillerIndices(0, 0, 0);

            foreach (var millerIndices in millerIndicesCollection)
            {
                if (millerIndices.AreEqual(
                    millerIndices0))
                    continue;

                if (millerIndicesWithoutFriedelPairs
                    .Exists(
                        m => m.AreEqual(
                            millerIndices,
                            true)))
                    continue;

                millerIndicesWithoutFriedelPairs.Add(millerIndices);
            }

            return millerIndicesWithoutFriedelPairs;
        }

        public static IXrdProfile CalculateXrdProfile(
            CrystalLattice crystalLattice, 
            IReadOnlyCollection<IAtomInUnitCell> atomsInUnitCell)
        {
            var selectedPeaks = CalculateXrdPeaks(
                crystalLattice, 
                atomsInUnitCell);

            var profile = CalculateXrdProfile(selectedPeaks
                .Select(p => (p.Q, p.I))
                .ToArray());

            return new XrdProfile
            {
                Peaks = selectedPeaks, 
                Profile = profile
            };
        }

        private static (double Q, double I, IMillerIndices Hkl)[] CalculateXrdPeaks(CrystalLattice crystalLattice,
            IReadOnlyCollection<IAtomInUnitCell> atomsInUnitCell)
        {
            var millerIndices = MillerIndices;

            var symMillerIndices = new List<List<IMillerIndices>>();

            foreach (var mi in millerIndices)
            {
                if (symMillerIndices.Any(
                    m => m.Any(
                        e => e.AreEqual(mi, true)))) continue;

                symMillerIndices.Add(
                    crystalLattice
                        .SymmetricalMillerIndices(mi)
                        .ToList());
            }

            var peaks = symMillerIndices.Select(l => (Hkl: l.First(), Multiplicity: l.Count))
                .Select(p => (
                    Q: 1 / crystalLattice.GetHklVector(p.Hkl).Norm,
                    p.Hkl,
                    p.Multiplicity))
                .Select(p => (
                    p.Q,
                    I: CalculateStructureFactorSqrt(atomsInUnitCell, p.Hkl, p.Q) * p.Multiplicity,
                    p.Hkl))
                .ToList();

            var threshold = peaks.Select(p => p.I).Max() * IntensityThreshold;
            var selectedPeaks = peaks.Where(p => p.I > threshold).ToArray();
            return selectedPeaks;
        }

        private static (double[] Q, double[] I) CalculateXrdProfile(
            IEnumerable<(double Q, double I)> peaks)
        {
            var qStep = (MaxQ - MinQ) / (NumberOfPoints - 1);
            var peakWidth = qStep * PeakWidth;
            var peakShapes = peaks
                .Select(
                    p => new PseudoVoigt(
                        p.Q, 
                        p.I,
                        peakWidth, 
                        0.5))
                .ToList();

            var q = new double[NumberOfPoints];
            var intensity = new double[NumberOfPoints];
            for (var i = 0; i < NumberOfPoints; i++)
            {
                q[i] = MinQ + qStep * i;
            }

            var partitioner = Partitioner.Create(0, NumberOfPoints);
            Parallel.ForEach(partitioner, range =>
            {
                var (u, v) = range;

                for (var i = u; i < v; i++)
                {
                    intensity[i] = peakShapes
                        .Select(
                            p => p
                                .Value(q[i]))
                        .Sum();
                }
            });

            return (Q: q, I: intensity);
        }

        private static double CalculateStructureFactorSqrt(
            IEnumerable<IAtomInUnitCell> atomsInUnitCell, 
            IMillerIndices millerIndices, double q)
        {
            var h = millerIndices.H;
            var k = millerIndices.K;
            var l = millerIndices.L;

            var structureFactor = 0.0;
            var cosPhase = 0.0;

            foreach (var atomInUnitCell in atomsInUnitCell)
            {
                var element = AtomicFormFactor.MapElements(atomInUnitCell.Labels);

                var aff = AtomicFormFactor.GetXRayAff(element, q) * atomInUnitCell.Occupancy;

                cosPhase += atomInUnitCell
                    .Positions
                    .Select(p => -2.0 * Math.PI * (h * p.X + k * p.Y + l * p.Z))
                    .Select(Math.Cos)
                    .Sum();

                structureFactor += aff * cosPhase;
            }

            return structureFactor * structureFactor / q;
        }
    }
}
