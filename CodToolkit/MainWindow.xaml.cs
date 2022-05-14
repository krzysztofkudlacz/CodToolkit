using System.Windows;
using CodToolkit.ModelView;

namespace CodToolkit
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
