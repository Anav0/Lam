using System.Windows;
using Projekt.ViewModels;

namespace Projekt
{
    /// <summary>
    ///     Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        public DialogWindowViewModel viewmodel { get; set; }
    }
}