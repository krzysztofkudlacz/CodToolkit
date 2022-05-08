using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Media;
using CodToolkit.Xrd;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace CodToolkit.ModelView
{
    public class AtomicFormFactorModelView : INotifyPropertyChanged
    {
        private const int NumberOfPoints = 1024;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AtomicFormFactorModelView()
        {
            Elements = new ObservableCollection<string>(
                AtomicFormFactor.GetElements());

            SelectedElement = Elements.First();
        }

        private string _selectedElement;
        private ObservableCollection<string> _elements;
        private SeriesCollection _seriesCollection;

        public string SelectedElement
        {
            get => _selectedElement;
            set
            {
                if(_selectedElement == value) return;
                _selectedElement = value;
                NotifyPropertyChanged();

                SeriesCollection = CalculateAtomicFormFactorProfile(
                    _selectedElement);
            }
        }

        public ObservableCollection<string> Elements
        {
            get => _elements;
            private set
            {
                _elements = value;
                NotifyPropertyChanged();
            }
        }

        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            private set
            {
                _seriesCollection = value;
                NotifyPropertyChanged();
            }
        }

        private static SeriesCollection CalculateAtomicFormFactorProfile(string element)
        {
            const double qMax = 20;
            const double qStep = qMax / (NumberOfPoints - 1);

            var q = new double[NumberOfPoints];

            for (var i = 0; i < NumberOfPoints; i++)
            {
                q[i] = i * qStep;
            }

            var intensity = AtomicFormFactor.GetXRayAff(element, q);

            var values = new ChartValues<ObservablePoint>();
            for (var i = 0; i < NumberOfPoints; i++)
            {
                values.Add(new ObservablePoint(q[i], intensity[i]));
            }

            var series = new LineSeries
            {
                Values = values,
                Title = "x-y data",
                StrokeThickness = 1,
                PointGeometry = null,
                LineSmoothness = 1,
                Stroke = Brushes.Blue,
                LabelPoint = point => $"({point.X:f2}, {point.Y:f2})",
                Fill = null,
            };

            var seriesCollection = new SeriesCollection
            {
                series
            };

            return seriesCollection;
        }
    }
}
