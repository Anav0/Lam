using System.Windows.Controls;
using System.Windows.Input;
using Projekt.ViewModels;

namespace Projekt.GUI.UserControls
{
    /// <summary>
    ///     Interaction logic for PresentationScreenControl.xaml
    /// </summary>
    public partial class PresentationScreenControl : UserControl
    {
        public PresentationScreenViewModel mViewModel {get; set; }

        public PresentationScreenControl(PresentationScreenViewModel mViewModel)
        {
            InitializeComponent();
            this.mViewModel = mViewModel;
            DataContext = mViewModel;
        }

    }
}