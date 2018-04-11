using System.Windows;

namespace Projekt
{
    /// <summary>
    /// Interaction logic for WaitingScreen.xaml
    /// </summary>
    public partial class WaitingScreen : Window
    {
        public WaitingScreen(WindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
        
    }
}
