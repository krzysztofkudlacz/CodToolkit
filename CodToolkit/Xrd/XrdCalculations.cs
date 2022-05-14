using System;
using System.Collections.Generic;
using System.Linq;
using CodToolkit.Crystallography;
using CodToolkit.Extensions;

namespace CodToolkit.Xrd
{
    public interface IXrdProfile
    {
        (double Q, double I, IMillerIndices)[] Peaks { get; }
    }

    public class XrdProfile : IXrdProfile
    {
        public (double Q, double I, IMillerIndices)[] Peaks { get; set; }
    }

    public static class XrdCalculations
    {
        private const double MaxQ = 20;
        private const int NumberOfPoints = 1024;
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
            var millerIndices = MillerIndices;

            var symMillerIndices = new List<List<IMillerIndices>>();

            foreach (var mi in millerIndices)
            {
                if(symMillerIndices.Any(
                    m => m.Any(
                        e => e.AreEqual(mi, true)))) continue;
                
                symMillerIndices.Add(
                    crystalLattice
                        .SymmetricalMillerIndices(mi)
                        .ToList());
            }

            var peaks = symMillerIndices.Select(l => l.First())
                .Select(m => (
                    Q: 1 / crystalLattice.GetHklVector(m).Norm, 
                    Hkl: m))
                .Select(g => (
                    g.Q, 
                    I: CalculateStructureFactorSqrt(atomsInUnitCell, g.Hkl, g.Q), 
                    g.Hkl))
                .ToList();

            var threshold = peaks.Select(p => p.I).Max() * IntensityThreshold;
            var selectedPeaks = peaks.Where(p => p.I > threshold).ToArray();

            return new XrdProfile {Peaks = selectedPeaks};
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
