using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CodToolkit.Cod;

namespace CodToolkit.ModelView
{
    public class CodModelView : INotifyPropertyChanged
    {
        private const string CodUri = "https://www.crystallography.net/cod/result";

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

        public async void SearchCod(CodSearchParameters parameters)
        {
            IndicateState(Cursors.Wait);

            CodEntries?.Clear();

            var results = await CodServerCommunication.SearchCod(parameters);

            var codEntries = new ObservableCollection<CodEntry>();
            foreach (var codEntry in results)
            {
                codEntries.Add(codEntry);
            }

            CodEntries = codEntries;

            IndicateState(null);
        }

        private static void IndicateState(Cursor cursor)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = cursor;
            });
        }

        public void CalculateXrdProfile()
        {

        }

        #endregion

    }
}
