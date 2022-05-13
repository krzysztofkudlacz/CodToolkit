using System.Windows;
using System.Windows.Input;
using CodToolkit.Cod;
using CodToolkit.ModelView;
using CodToolkit.ModelView.Commands;
using CodToolkit.View;
using MaterialDesignThemes.Wpf;

namespace CodToolkit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ICommand ShowCodDialogCommand { get; private set; }

        public static ICommand CloseCodDialogCommand { get; private set; }

        public static ICommand SearchCodCommand { get; private set; }

        public static ICommand DownloadCifCommand { get; private set; }

        private readonly CodModelView _codModelView;

        private readonly CodSearchParameters _searchParameters;

        public App()
        {
            _codModelView = new CodModelView();
            _searchParameters = new CodSearchParameters();

            ShowCodDialogCommand = new ShowSearchCodDialogCommand(
                () => true,
                ShowSearchCodDialog);

            CloseCodDialogCommand = new CloseSearchCodDialogCommand(
                () => true, 
                CancelCodSearchDialog);

            SearchCodCommand = new SearchCodCommand(
                () => true,
                StartCodSearch);

            DownloadCifCommand = new DownloadCifCommand(
                () => _codModelView.SelectedCodEntry != null,
                _codModelView.DownloadCif);
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow
            {
                CodModelView = _codModelView
            };
            mainWindow.Show();
        }

        private async void ShowSearchCodDialog()
        {
            var control = new SearchCodConditions
            {
                SearchParameters = _searchParameters
            };

            var result = await DialogHost.Show(control);
            if ((bool?)result == true)
            {
                IndicateState(Cursors.Wait);
                _codModelView.SearchCod(_searchParameters);
                IndicateState(null);
            }
        }

        private static void StartCodSearch()
        {
            DialogHost.CloseDialogCommand.Execute(true, null);
        }

        private static void CancelCodSearchDialog()
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
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
