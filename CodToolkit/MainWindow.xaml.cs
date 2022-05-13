using System.Windows;
using CodToolkit.ModelView;

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

            var str = "-z-y";
            str = str.Replace("x", "0.5").Replace("y", "0.5").Replace("z", "0.5");
            var parser = new Mathos.Parser.MathParser();
            var val = parser.Parse(str) % 1;
        }

        public static readonly DependencyProperty CodModelViewProperty = DependencyProperty.Register(
            nameof(CodModelView), 
            typeof(CodModelView),
            typeof(MainWindow), 
            new PropertyMetadata(default(CodModelView)));
        
        public CodModelView CodModelView
        {
            get => (CodModelView) GetValue(CodModelViewProperty);
            set => SetValue(CodModelViewProperty, value);
        }
    }
}
