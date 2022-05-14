using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CodToolkit.Xrd
{
    public static class AtomicFormFactor
    {
        private static IReadOnlyDictionary<string, (
            double a1,
            double b1,
            double a2,
            double b2,
            double a3,
            double b3,
            double a4,
            double b4,
            double c)> _atomicFormFactors;

        private static (
            double a1,
            double b1,
            double a2,
            double b2,
            double a3,
            double b3,
            double a4,
            double b4,
            double c) GetFormFactorCoefficients(string element)
        {
            _atomicFormFactors ??= GetFormFactorCoefficients();

            return _atomicFormFactors[element];
        }

        public static IEnumerable<string> GetElements()
        {
            _atomicFormFactors ??= GetFormFactorCoefficients();
            return _atomicFormFactors.Keys;
        }

        public static string MapElements(string[] candidates)
        {
            var elements = GetElements();

            var element = elements
                .FirstOrDefault(
                    e => candidates.Any(c => c == e || e.Contains(c) || c.Contains(e)));

            if (!string.IsNullOrEmpty(element)) return element;

            throw new Exception($"Cannot map any of the elements: ({string.Join(",", candidates)})");
        }

        public static double GetXRayAff(string element, double theta, double wavelength)
        {
            var s = Math.Sin(theta) / wavelength;

            var aff = GetFormFactorCoefficients(element);

            var fx = 0.0;
            var s2 = Math.Pow(s, 2.0);

            fx += aff.a1 * Math.Exp(-aff.b1 * s2);
            fx += aff.a2 * Math.Exp(-aff.b2 * s2);
            fx += aff.a3 * Math.Exp(-aff.b3 * s2);
            fx += aff.a4 * Math.Exp(-aff.b4 * s2);
            fx += aff.c;

            return fx;
        }

        // q should be between 0 and 25^-1 Angstrom
        public static double GetXRayAff(string element, double q)
        {
            const double fourPi = 4 * Math.PI;

            var aff = GetFormFactorCoefficients(element);

            var fx = 0.0;
            var s2 = Math.Pow(q / fourPi, 2.0);

            fx += aff.a1 * Math.Exp(-aff.b1 * s2);
            fx += aff.a2 * Math.Exp(-aff.b2 * s2);
            fx += aff.a3 * Math.Exp(-aff.b3 * s2);
            fx += aff.a4 * Math.Exp(-aff.b4 * s2);
            fx += aff.c;

            return fx;
        }

        public static double[] GetXRayAff(string element, double[] q)
        {
            const double fourPi = 4 * Math.PI;

            var aff = GetFormFactorCoefficients(element);

            var fx = new double[q.Length];

            for (var i = 0; i < q.Length; i++)
            {
                var s2 = Math.Pow(q[i] / fourPi, 2.0);

                fx[i] += aff.a1 * Math.Exp(-aff.b1 * s2);
                fx[i] += aff.a2 * Math.Exp(-aff.b2 * s2);
                fx[i] += aff.a3 * Math.Exp(-aff.b3 * s2);
                fx[i] += aff.a4 * Math.Exp(-aff.b4 * s2);
                fx[i] += aff.c;
            }

            return fx;
        }

        private static IReadOnlyDictionary<string, (
            double a1,
            double b1,
            double a2,
            double b2,
            double a3,
            double b3,
            double a4,
            double b4,
            double c)> GetFormFactorCoefficients()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourcePath = assembly
                .GetManifestResourceNames()
                .Single(str => str.EndsWith("AtomicFormFactors.txt"));

            using var stream = assembly.GetManifestResourceStream(resourcePath);
            using var reader =
                new StreamReader(stream ??
                                 throw new ArgumentException("Cannot read atomic form factors"));

            reader.ReadLine();

            var atomicFormFactors = new Dictionary
            <string, (
                double a1,
                double b1,
                double a2,
                double b2,
                double a3,
                double b3,
                double a4,
                double b4,
                double c)>();

            while (!reader.EndOfStream)
            {
                var input = reader
                    .ReadLine()?
                    .Split("\t".ToCharArray())
                    .Select(t => t.Trim())
                    .ToArray();
                 
                if(input == null) continue;

                atomicFormFactors.Add(
                    input[0], (
                        double.Parse(input[1]),
                        double.Parse(input[2]),
                        double.Parse(input[3]),
                        double.Parse(input[4]),
                        double.Parse(input[5]),
                        double.Parse(input[6]),
                        double.Parse(input[7]),
                        double.Parse(input[8]),
                        double.Parse(input[9])));
            }

            return atomicFormFactors;
        }
    }
}
