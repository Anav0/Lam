using System;
using System.Linq;
using System.Windows;

namespace Projekt.GUI.Windows
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void onClose(object sender, EventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                Visibility = Visibility.Hidden;
                Serializer serializer = new Serializer();

                foreach (var dtmcgroup in viewModel.SavedDtmcData.List)
                {
                    var modifiedGroup = serializer.ReadFromJsonFile<DtmcGroup>(dtmcgroup.FilePath);
                    modifiedGroup.GivenName = dtmcgroup.FileName;

                    viewModel.StoreGroup(modifiedGroup, dtmcgroup.FilePath);
                }

                foreach (var resultGroup in viewModel.SavedResultsData.List)
                {
                    var modifiedGroup = serializer.ReadFromJsonFile<TestedGroup>(resultGroup.FilePath);
                    modifiedGroup.GivenName = resultGroup.FileName;

                    viewModel.StoreGroup(modifiedGroup, resultGroup.FilePath);
                }
            }
        }
    }
}