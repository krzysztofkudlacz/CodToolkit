using System;

namespace CodToolkit.Crystallography
{
    public interface IMillerIndices
    {
        int H { get; set; }

        int K { get; set; }

        int L { get; set; }

        IMillerIndices FriedelPair { get; }

        bool Equals(IMillerIndices millerIndices, bool includeFriedelPair = false);
    }

    public class MillerIndices : IMillerIndices
    {
        public int H { get; set; }

        public int K { get; set; }

        public int L { get; set; }

        public MillerIndices()
        {
            
        }

        public MillerIndices(int h, int k, int l)
        {
            H = h;
            K = k;
            L = l;
        }

        public IMillerIndices FriedelPair => new MillerIndices(-H, -K, -L);

        public bool Equals(IMillerIndices millerIndices, bool includeFriedelPair = false)
        {
            return includeFriedelPair
                ? Math.Abs(H - millerIndices.H) == 0 && Math.Abs(K - millerIndices.K) == 0 &&
                  Math.Abs(L - millerIndices.L) == 0 ||
                  Math.Abs(H + millerIndices.H) == 0 && Math.Abs(K + millerIndices.K) == 0 &&
                  Math.Abs(L + millerIndices.L) == 0
                : Math.Abs(H - millerIndices.H) == 0 && Math.Abs(K - millerIndices.K) == 0 &&
                  Math.Abs(L - millerIndices.L) == 0;
        }
    }
}
