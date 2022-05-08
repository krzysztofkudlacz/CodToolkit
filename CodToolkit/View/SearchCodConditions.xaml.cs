using System.Windows;
using System.Windows.Controls;
using CodToolkit.Cod;

namespace CodToolkit.View
{
    /// <summary>
    /// Interaction logic for SearchCodConditions.xaml
    /// </summary>
    public partial class SearchCodConditions : UserControl
    {
        public SearchCodConditions()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SearchParametersProperty = DependencyProperty.Register(
            nameof(SearchParameters), 
            typeof(CodSearchParameters), 
            typeof(SearchCodConditions), 
            new PropertyMetadata(default(CodSearchParameters)));

        public CodSearchParameters SearchParameters
        {
            get => (CodSearchParameters) GetValue(SearchParametersProperty);
            set => SetValue(SearchParametersProperty, value);
        }
    }
}
