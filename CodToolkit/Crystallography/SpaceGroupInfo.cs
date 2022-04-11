namespace CodToolkit.Crystallography
{
    public interface ISpaceGroupInfo
    {
        string HallName { get; }

        string HermannMaguinName { get; }

        string LaueClassSymbol { get; }
    }

    public class SpaceGroupInfo : ISpaceGroupInfo
    {
        public string HallName { get; set; }

        public string HermannMaguinName { get; set; }

        public string LaueClassSymbol { get; set; }
    }
}
