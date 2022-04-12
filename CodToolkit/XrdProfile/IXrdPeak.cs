namespace CodToolkit.XrdProfile
{
    public interface IXrdPeak
    {
        public double Center { get; }

        public double Fwhm { get; }

        public double Height { get; }

        public double Value(double x);

        public double[] Values(double[] x);
    }
}
