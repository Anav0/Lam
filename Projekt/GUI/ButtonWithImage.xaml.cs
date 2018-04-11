using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Projekt.GUI
{
    /// <summary>
    /// Interaction logic for ButtonWithImage.xaml
    /// </summary>
    public partial class ButtonWithImage : UserControl
    {
        public ButtonWithImage()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        #region ButtonContent DP
        public string ButtonContent
        {
            get { return (string)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }

        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register("ButtonContent", typeof(string),
              typeof(ButtonWithImage), new PropertyMetadata(null));
        #endregion

        #region MyImageSource DP
        public ImageSource MyImageSource
        {
            get { return (ImageSource)GetValue(MyImageSourceProperty); }
            set { SetValue(MyImageSourceProperty, value); }
        }

        public static readonly DependencyProperty MyImageSourceProperty =
            DependencyProperty.Register("MyImageSource", typeof(ImageSource),
              typeof(ButtonWithImage), new PropertyMetadata(null));
        #endregion

        #region MaxiCommand DP
        public RelayCommand MaxiCommand
        {
            get { return (RelayCommand)GetValue(MaxiCommandProperty); }
            set { SetValue(MaxiCommandProperty, value); }
        }

        public static readonly DependencyProperty MaxiCommandProperty =
            DependencyProperty.Register("MaxiCommand", typeof(RelayCommand),
              typeof(ButtonWithImage), new PropertyMetadata(null));
        #endregion

        #region MaxiCommandParameter DP
        public object MaxiCommandParameter
        {
            get { return (object)GetValue(MaxiCommandParameterProperty); }
            set { SetValue(MaxiCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty MaxiCommandParameterProperty =
            DependencyProperty.Register("MaxiCommandParameter", typeof(object),
              typeof(ButtonWithImage), new PropertyMetadata(null));
        #endregion
    }
}
