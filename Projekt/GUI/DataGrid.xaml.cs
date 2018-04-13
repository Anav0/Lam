using Projekt.ViewModels;

namespace Projekt.GUI
{
    /// <summary>
    ///     Interaction logic for DataGrid.xaml
    /// </summary>
    public partial class DataGrid
    {
        public DataGrid()
        {
            InitializeComponent();
            DataContext = new DataGridViewModel();
        }
    }
}