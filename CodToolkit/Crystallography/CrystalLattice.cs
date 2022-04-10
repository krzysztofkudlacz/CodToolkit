namespace CodToolkit.Crystallography
{
    public class CrystalLattice
    {
        public ICrystalLatticeParameters CrystalLatticeParameters { get; }

        public ISpaceGroupInfo SpaceGroupInfo { get; }

        public CrystalLattice(ICrystalLatticeParameters crystalLatticeParameters, 
            ISpaceGroupInfo spaceGroupInfo)
        {
            CrystalLatticeParameters = crystalLatticeParameters;
            SpaceGroupInfo = spaceGroupInfo;

            
        }
    }
}
