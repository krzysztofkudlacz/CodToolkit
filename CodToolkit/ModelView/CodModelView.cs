using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CodToolkit.Cod;

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

            var results = await CodServerCommunication.DownloadCif(fileId);
        }

        #endregion

    }
}
