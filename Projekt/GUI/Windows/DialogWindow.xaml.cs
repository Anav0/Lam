using System.Windows;
using System.Windows.Input;

namespace Projekt
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindowViewModel viewmodel { get; set; }

        public DialogWindow()
        {
            InitializeComponent();
        }

    }
}
