using System.Windows.Controls;
using Projekt.ViewModels;

namespace Projekt
{
    /// <summary>
    ///     Interaction logic for Results.xaml
    /// </summary>
    public partial class Results : UserControl
    {
        public Results(ResultsViewModel viewmodel)
        {
            InitializeComponent();
            DataContext = viewmodel;
        }
    }
}