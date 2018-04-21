using System.Windows.Controls;
using System.Windows.Input;
using Projekt.ViewModels;

namespace Projekt.GUI.UserControls
{
    /// <summary>
    ///     Interaction logic for EndResultsControl.xaml
    /// </summary>
    public partial class EndResultsControl : UserControl
    {
        public EndResultsViewModel mViewModel {get; set; }

        public EndResultsControl(EndResultsViewModel mViewModel)
        {
            InitializeComponent();
            this.mViewModel = mViewModel;
            DataContext = mViewModel;
        }

    }
}