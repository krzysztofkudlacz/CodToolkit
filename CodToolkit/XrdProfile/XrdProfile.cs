using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodToolkit.XrdProfile
{
    public class XrdProfile
    {
        private readonly IReadOnlyList<IXrdPeak> _xrdPeaks;

        public XrdProfile(IReadOnlyList<IXrdPeak> xrdPeaks)
        {
            _xrdPeaks = xrdPeaks;
        }

        public double Value(double x)
        {
            return _xrdPeaks.Sum(xrdPeak => xrdPeak.Value(x));
        }

        public double[] Values(double[] x)
        {
            var partitioner = Partitioner.Create(0, x.Length);
            var results = new double[x.Length];

            Parallel.ForEach(partitioner, (range, loopSate) =>
            {
                var (index0, index1) = range;
                for (var i = index0; i < index1; i++)
                {
                    results[i] += Value(x[i]);
                }
            });

            return results;
        }
    }
}
