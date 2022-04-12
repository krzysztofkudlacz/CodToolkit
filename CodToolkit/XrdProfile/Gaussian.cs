using System.ComponentModel;
using System.Linq;

namespace CodToolkit.XrdProfile
{
    [Description("Gaussian")]
    public class Gaussian : IXrdPeak
    {
        public Gaussian(
            double center, 
            double height, 
            double fwhm)
        {
            Center = center;
            Height = height;
            Fwhm = fwhm;
        }

        public double Center { get; }

        public double Fwhm { get; }

        public double Height { get; }

        private static readonly double FwhmConst = 4.0 * System.Math.Log(2.0);

        public double Value(double x)
        {
            var b02 = System.Math.Pow(Fwhm, 2.0);
            var dx2 = System.Math.Pow(x - Center, 2.0);

            return Height * System.Math.Exp(-FwhmConst * dx2 / b02);
        }

        public double[] Values(double[] x)
        {
            return x.Select(Value).ToArray();
        }
    }
}
