using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CodToolkit.Cod
{
    public class CodSearchParameters : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CodSearchParameters()
        {
            Text = "dolomite";
            RequiredElements = "Al"; // "Ca, Mg, C, O";
            ExcludedElements = "H, Cu, B, Si";
            MinA = "4.04"; // "4.8";
            MaxA = "4.06"; // "5";
            MinB = "4.04"; // "4.8";
            MaxB = "4.06"; //  "5";
            MinC = "4.04"; // "15";
            MaxC = "4.06"; // "16";
        }

        private string _text;
        private string _requiredElements;
        private string _excludedElements;
        private string _minNumberOfDistinctElements;
        private string _maxNumberOfDistinctElements;

        private string _minA;
        private string _minB;
        private string _minC;

        private string _maxA;
        private string _maxB;
        private string _maxC;

        private string _minAlpha;
        private string _minBeta;
        private string _minGamma;

        private string _maxAlpha;
        private string _maxBeta;
        private string _maxGamma;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                NotifyPropertyChanged();
            }
        }

        public string RequiredElements
        {
            get => _requiredElements;
            set
            {
                _requiredElements = value; 
                NotifyPropertyChanged();
            }
        }

        public string ExcludedElements
        {
            get => _excludedElements;
            set
            {
                _excludedElements = value; 
                NotifyPropertyChanged();
            }
        }

        public string MinNumberOfDistinctElements
        {
            get => _minNumberOfDistinctElements;
            set
            {
                _minNumberOfDistinctElements = value; 
                NotifyPropertyChanged();
            }
        }

        public string MaxNumberOfDistinctElements
        {
            get => _maxNumberOfDistinctElements;
            set
            {
                _maxNumberOfDistinctElements = value; 
                NotifyPropertyChanged();
            }
        }

        public string MinA
        {
            get => _minA;
            set
            {
                _minA = value;
                NotifyPropertyChanged();
            }
        }

        public string MaxA
        {
            get => _maxA;
            set
            {
                _maxA = value;
                NotifyPropertyChanged();
            }
        }

        public string MinB
        {
            get => _minB;
            set
            {
                _minB = value;
                NotifyPropertyChanged();
            }
        }

        public string MaxB
        {
            get => _maxB;
            set
            {
                _maxB = value;
                NotifyPropertyChanged();
            }
        }

        public string MinC
        {
            get => _minC;
            set
            {
                _minC = value;
                NotifyPropertyChanged();
            }
        }

        public string MaxC
        {
            get => _maxC;
            set
            {
                _maxC = value;
                NotifyPropertyChanged();
            }
        }

        public string MinAlpha
        {
            get => _minAlpha;
            set
            {
                _minAlpha = value;
                NotifyPropertyChanged();
            }
        }

        public string MaxAlpha
        {
            get => _maxAlpha;
            set
            {
                _maxAlpha = value;
                NotifyPropertyChanged();
            }
        }

        public string MinBeta
        {
            get => _minBeta;
            set
            {
                _minBeta = value;
                NotifyPropertyChanged();
            }
        }

        public string MaxBeta
        {
            get => _maxBeta;
            set
            {
                _maxBeta = value;
                NotifyPropertyChanged();
            }
        }

        public string MinGamma
        {
            get => _minGamma;
            set
            {
                _minGamma = value;
                NotifyPropertyChanged();
            }
        }

        public string MaxGamma
        {
            get => _maxGamma;
            set
            {
                _maxGamma = value;
                NotifyPropertyChanged();
            }
        }

        public CodSearchParameters Clone()
        {
            var clone = new CodSearchParameters();
            var properties = GetType().GetProperties();

            foreach (var property in properties)
            {
                property.SetValue(clone, property.GetValue(this));
            }

            return clone;
        }
    }
}
