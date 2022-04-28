using System.Windows;
using System.Windows.Input;
using CodToolkit.Cod;
using CodToolkit.ModelView;
using CodToolkit.Xrd;

namespace CodToolkit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public static readonly RoutedCommand QueryCodCommand = new RoutedCommand();
        //private readonly CodServerCommunication _codServerCommunication;

        public MainWindow()
        {
            InitializeComponent();

            AtomicFormFactorModelView = new AtomicFormFactorModelView();
        }

        public static readonly DependencyProperty AtomicFormFactorModelViewProperty = DependencyProperty.Register(
            nameof(AtomicFormFactorModelView), 
            typeof(AtomicFormFactorModelView),
            typeof(MainWindow), 
            new PropertyMetadata(default(AtomicFormFactorModelView)));

        public AtomicFormFactorModelView AtomicFormFactorModelView
        {
            get => (AtomicFormFactorModelView) GetValue(AtomicFormFactorModelViewProperty);
            set => SetValue(AtomicFormFactorModelViewProperty, value);
        }

        /*
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
        */
    }
}
