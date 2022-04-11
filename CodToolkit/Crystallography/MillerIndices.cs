using System.Collections.Generic;
using System.Linq;

namespace CodToolkit.Crystallography
{
    public interface IMillerIndices
    {
        int H { get; }

        int K { get; }

        int L { get; }

        IMillerIndices FriedelPair { get; }

        bool IsEqual(IMillerIndices millerIndices, bool includeFriedelPair = false);
    }

    public class MillerIndices : IMillerIndices
    {
        public int H { get; set; }

        public int K { get; set; }

        public int L { get; set; }

        public MillerIndices()
        {
        }

        public MillerIndices(
            int h, 
            int k, 
            int l)
        {
            H = h;
            K = k;
            L = l;
        }

        public IMillerIndices FriedelPair => new MillerIndices(-H, -K, -L);

        public bool IsEqual(
            IMillerIndices millerIndices, 
            bool asFriedelPair = false)
        {
            return asFriedelPair
                ? AreEqual(this, millerIndices) || 
                  AreEqual(this, millerIndices.FriedelPair)
                : AreEqual(this, millerIndices);
        }

        public static bool AreEqual(
            IMillerIndices millerIndices1,
            IMillerIndices millerIndices2)
        {
            var list1 = AsList(millerIndices1);
            var list2 = AsList(millerIndices2);

            return list1.Zip(
                    list2,
                    (i1, i2) => i1 - i2)
                .Sum() == 0;
        }

        private static IEnumerable<int> AsList(
            IMillerIndices millerIndices) =>
            new List<int>
            {
                millerIndices.H, 
                millerIndices.K,
                millerIndices.L
            };
    }
}
