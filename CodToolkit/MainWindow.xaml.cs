using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using BioCif.Core.Parsing;
using CodToolkit.Cod;
using CodToolkit.CrystallographicInformationFile;
using CodToolkit.XrdProfile;
using Flurl;
using Flurl.Http;

namespace CodToolkit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly RoutedCommand QueryCodCommand = new RoutedCommand();

        private readonly CodServerCommunication _codServerCommunication;

        public MainWindow()
        {
            InitializeComponent();

            /*
            _codServerCommunication = new CodServerCommunication();

            var xrdProfile = new XrdProfile.XrdProfile(new List<IXrdPeak> {new Gaussian(5, 1, 1)});
            var x = new double[100];
            for (var i = 0; i < x.Length; i++)
            {
                x[i] = i;
            }

            xrdProfile.Values(x);
            */
        }

        private async void QueryCodExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            IndicateState(Cursors.Wait);

            var codUrl = new Url("https://www.crystallography.net/cod/result");
            //codUrl.SetQueryParam("id", "9000583");
            codUrl.SetQueryParam("id", "1000044");
            codUrl.SetQueryParam("format", "zip");

            var zippedBuffer = await codUrl.GetAsync().ReceiveBytes();

            var entry = new ZipArchive(
                    new MemoryStream(zippedBuffer))
                .Entries
                .First();

            var memoryStream = new MemoryStream();
            await entry.Open().CopyToAsync(memoryStream);

            var cifText = Encoding.Default.GetString(memoryStream.ToArray());

            var cif = CrystallographicInformationFile.CrystallographicInformationFile.Parse(
                new MemoryStream(Encoding.Unicode.GetBytes(cifText)));

            /*
            var codEntries = await _codServerCommunication.QueryCod();

            CodEntries.ItemsSource = codEntries.Select(entry => new
            {
                Number = entry.NumberInCollection, 
                Id = entry.FileId,
                Parameters = entry.CellParameters,
                entry.Formula,
                entry.SpaceGroup
            });
            */

            IndicateState(null);
        }

        private void CanQueryCod(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void IndicateState(Cursor cursor)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = cursor;
            });
        }
    }
}
