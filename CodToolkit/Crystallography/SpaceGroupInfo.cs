using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodToolkit.Crystallography
{
    public interface ISpaceGroupInfo
    {
        string Symbol { get; }

        string ExtendedSymbol { get; }

        string LaueClass { get; }
    }

    public class SpaceGroupInfo : ISpaceGroupInfo
    {
        public string Symbol { get; set; }

        public string ExtendedSymbol { get; set; }

        public string LaueClass { get; set; }
    }
}
