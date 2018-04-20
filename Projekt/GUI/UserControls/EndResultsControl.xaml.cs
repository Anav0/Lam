using System.Windows.Controls;

namespace Projekt
{
    /// <summary>
    /// Interaction logic for EndResultsControl.xaml
    /// </summary>
    public partial class EndResultsControl : UserControl
    {
        public EndResultsControl(EndResultsViewModel viewmodel)
        {
            InitializeComponent();
            DataContext = viewmodel;
        }
    }
}
