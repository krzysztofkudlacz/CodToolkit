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
using CodToolkit.Crystallography;

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

            while (!reader.EndOfStream)
            {
                var input = reader.
                    ReadLine()?.
                    Split(";".ToCharArray());

                if (input == null) continue;

                var spaceGroupInfo = new SpaceGroupInfo
                {
                    LaueClass = input[0].Trim(),
                    Symbol = input[3].Trim(),
                    ExtendedSymbol = input[4].Trim()
                };

                if (spaceGroupInfo.Symbol.StartsWith("R"))
                {

                }
            }
        }
    }
}
