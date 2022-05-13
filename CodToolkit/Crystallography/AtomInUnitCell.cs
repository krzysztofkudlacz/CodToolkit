using System.Collections.Generic;

namespace CodToolkit.Crystallography
{
    public interface IAtomInUnitCell
    {
        string[] Labels { get; }

        double Occupancy { get; }

        IReadOnlyList<(double X, double Y, double Z)> Positions { get; }
    }

    public class AtomInUnitCell : IAtomInUnitCell
    {
        public string[] Labels { get; set; }

        public double Occupancy { get; set; }

        public IReadOnlyList<(double X, double Y, double Z)> Positions { get; set; }
    }
}
