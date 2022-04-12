using System.ComponentModel;
using System.Linq;

namespace CodToolkit.XrdProfile
{
    [Description("Lorentzian")]
    public class Lorentzian : IXrdPeak
    {
        public Lorentzian(
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

        public double Value(double x)
        {
            var b0 = 0.5 * Fwhm;
            var b02 = b0 * b0;
            var dx2 = System.Math.Pow(x - Center, 2.0);

            return Height * b02 / (dx2 + b02);
        }

        public double[] Values(double[] x)
        {
            return x.Select(Value).ToArray();
        }
    }
}
