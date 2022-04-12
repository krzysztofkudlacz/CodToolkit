using System.ComponentModel;
using System.Linq;

namespace CodToolkit.XrdProfile
{
    [Description("Pseudo-Voigt")]
    public class PseudoVoigt : IXrdPeak
    {
        private readonly IXrdPeak _lorentzian;
        private readonly IXrdPeak _gaussian;

        public PseudoVoigt(
            double center, 
            double height, 
            double fwhm, 
            double gamma)
        {
            Center = center;
            Height = height;
            Fwhm = fwhm;
            Gamma = gamma;

            _lorentzian = new Lorentzian(Center, Height, Fwhm);
            _gaussian = new Gaussian(Center, Height, Fwhm);
        }

        public double Center { get; }

        public double Fwhm { get; }

        public double Height { get; }

        public double Gamma { get; }

        public double Value(double x)
        {
            return Gamma * _gaussian.Value(x) + 
                   (1 - Gamma) * _lorentzian.Value(x);
        }

        public double[] Values(double[] x)
        {
            return x.Select(Value).ToArray();
        }
    }
}
