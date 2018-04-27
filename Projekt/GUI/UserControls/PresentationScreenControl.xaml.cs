using System.Windows.Controls;

namespace Projekt.GUI.UserControls
{
    /// <summary>
    ///     Interaction logic for PresentationScreenControl.xaml
    /// </summary>
    public partial class PresentationScreenControl : UserControl
    {
        public PresentationScreenControl(PresentationScreenViewModel mViewModel)
        {
            InitializeComponent();
            this.mViewModel = mViewModel;
            DataContext = mViewModel;
        }

        public PresentationScreenViewModel mViewModel { get; set; }
    }
}