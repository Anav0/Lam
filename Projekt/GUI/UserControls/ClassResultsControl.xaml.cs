using System.Windows.Controls;
using Projekt.ViewModels;

namespace Projekt.GUI.UserControls
{
    /// <summary>
    ///     Interaction logic for ClassResultsControl.xaml
    /// </summary>
    public partial class ClassResultsControl : UserControl
    {
        public ClassResultsControl(ClassResultsViewModel viewmodel)
        {
            InitializeComponent();
            DataContext = viewmodel;
        }
    }
}