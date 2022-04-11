using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using CodToolkit.Algebra;
using CodToolkit.Crystallography;
using CodToolkit.LaueClass;

namespace CodToolkit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();

            var resourcePath = assembly
                .GetManifestResourceNames()
                .Single(str => str.EndsWith("SpaceGroupToLaueMap.txt"));

            using var stream = assembly.GetManifestResourceStream(resourcePath);
            using var reader =
                new StreamReader(stream ?? 
                                 throw new ArgumentException("Cannot read space group to Laue class map"));

            var spaceGroupInfos = new List<SpaceGroupInfo>();

            while (!reader.EndOfStream)
            {
                var input = reader.
                    ReadLine()?.
                    Split(";".ToCharArray());

                if (input == null) continue;

                spaceGroupInfos.Add(new SpaceGroupInfo
                {
                    LaueClassSymbol = input[0].Trim(),
                    HallName = input[3].Trim(),
                    HermannMaguinName = input[4].Trim()
                });
            }

            var crystalLattice = new CrystalLattice(
                new CrystalLatticeParameters
                {
                    ConstA = 1, 
                    ConstB = 1, 
                    ConstC = 1, 
                    Alpha = 90,
                    Beta = 90, 
                    Gamma = 90
                },
                spaceGroupInfos.Last());

            var h = crystalLattice.TransitionMatrix;
            var ht = h.Transpose();
            var g = ht.Inverse();
            var gt = g.Transpose();

            var mH = Matrix3X3.Multiply(h, ht);
            var mG = Matrix3X3.Multiply(g, gt);

            var equal = Matrix3X3.AreEqual(mH, mG.Inverse(), 0.0001);

            var results = crystalLattice.SymmetricalMillerIndices(new MillerIndices(1, 0, 0));

            var laueClass = LaueClassCreator.CreateLaueClass(spaceGroupInfos.Last().LaueClassSymbol);

            var isEqual = new MillerIndices(1, 1, 1).IsEqual(new MillerIndices(-1, -1, -1), true);
        }
    }
}
