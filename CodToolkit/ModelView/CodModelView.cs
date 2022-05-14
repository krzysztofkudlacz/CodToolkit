using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using CodToolkit.Cod;
using CodToolkit.Crystallography;
using CodToolkit.Xrd;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace CodToolkit.ModelView
{
    public class CodModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Search COD

        private ObservableCollection<CodEntry> _codEntries;

        public ObservableCollection<CodEntry> CodEntries
        {
            get => _codEntries;
            private set
            {
                _codEntries = value;
                NotifyPropertyChanged();
            }
        }

        private CodEntry _selectedCodEntry;

        public CodEntry SelectedCodEntry
        {
            get => _selectedCodEntry;
            set
            {
                _selectedCodEntry = value;
                NotifyPropertyChanged();
            }
        }

        private SeriesCollection _seriesCollection;

        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            private set
            {
                _seriesCollection = value;
                NotifyPropertyChanged();
            }
        }

        public async void SearchCod(CodSearchParameters parameters)
        {
            CodEntries?.Clear();

            var results = await CodServerCommunication.SearchCod(parameters);

            var codEntries = new ObservableCollection<CodEntry>();
            foreach (var codEntry in results)
            {
                codEntries.Add(codEntry);
            }

            CodEntries = codEntries;
        }

        public async void DownloadCif()
        {
            var fileId = SelectedCodEntry?.FileId;
            if (string.IsNullOrEmpty(fileId)) return;

            var cif = await CodServerCommunication.DownloadCif(fileId);

            var crystalLattice = new CrystalLattice(
                cif.Parameters, 
                cif.SpaceGroupSymbols);

            var peaks = XrdCalculations.CalculateXrdProfile(
                crystalLattice, 
                cif.AtomsInUnitCell).Peaks;

            var values = new ChartValues<ObservablePoint>();
            for (var i = 0; i < peaks.Length; i++)
            {
                values.Add(new ObservablePoint(peaks[i].Q, peaks[i].I));
            }

            var series = new ColumnSeries()
            {
                Values = values,
                Title = "peaks",
                MaxColumnWidth = 6,
                StrokeThickness = 1,
                PointGeometry = null,
                Stroke = Brushes.Blue,
                Fill = Brushes.Blue,
                LabelPoint = point => "", //$"({point.X}, {point.Y})",
                DataLabels = true,
                SharesPosition = false,
                ToolTip = null
            };

            SeriesCollection = new SeriesCollection
            {
                series
            };
        }

        #endregion

    }
}
