using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CodToolkit.Cod;

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

            _codServerCommunication = new CodServerCommunication();
        }

        private async void QueryCodExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });

            var codEntries = await _codServerCommunication.QueryCod();

            CodEntries.ItemsSource = codEntries.Select(entry => new
            {
                Number = entry.NumberInCollection, 
                Id = entry.FileId,
                Parameters = entry.CellParameters,
                entry.Formula,
                entry.SpaceGroup
            });

            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
        }

        private void CanQueryCod(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
