using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodToolkit.Crystallography
{
    public interface ICrystalLatticeParameters
    {
        double ConstA { get; }

        double ConstB { get; }

        double ConstC { get; }

        double Alpha { get; }

        double Beta { get; }

        double Gamma { get; }
    }

    public class CrystalLatticeParameters : ICrystalLatticeParameters
    {
        public double ConstA { get; set; }

        public double ConstB { get; set; }

        public double ConstC { get; set; }

        public double Alpha { get; set; }

        public double Beta { get; set; }

        public double Gamma { get; set; }
    }
}
