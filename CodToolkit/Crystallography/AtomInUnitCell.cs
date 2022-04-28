using System.Collections.Generic;

namespace CodToolkit.Crystallography
{
    public interface IAtomInUnitCell
    {
        string Label { get; }

        double Occupancy { get; }

        IReadOnlyList<(double X, double Y, double Z)> Positions { get; }
    }

    public class AtomInUnitCell : IAtomInUnitCell
    {
        public string Label { get; set; }

        public double Occupancy { get; set; }

        public IReadOnlyList<(double X, double Y, double Z)> Positions { get; set; }
    }
}
