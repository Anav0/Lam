using Projekt.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Projekt.GUI
{
    /// <summary>
    /// Interaction logic for DataGridWithLabel.xaml
    /// </summary>
    public partial class DataGridWithLabel : UserControl
    {
        public DataGridWithLabel()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }
        #region Label DP
        public string Label
        {
            get
            {
                return (String)GetValue(LabelProperty);
            }
            set
            {
                SetValue(LabelProperty, value);
            }
        }
       
        public static readonly DependencyProperty LabelProperty =
      DependencyProperty.Register("Label", typeof(string),
        typeof(DataGridWithLabel), new PropertyMetadata(""));
        #endregion

        #region Data DP
        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object),
              typeof(DataGridWithLabel), new PropertyMetadata(null));

        #endregion

        #region Width DP
        public float ControlWidth
        {
            get { return (float)GetValue(ControlWidthProperty); }
            set { SetValue(ControlWidthProperty, value); }
        }

        public static readonly DependencyProperty ControlWidthProperty =
            DependencyProperty.Register("ControlWidth", typeof(float),
              typeof(DataGridWithLabel), new PropertyMetadata(null));
        #endregion

        #region Height DP
        public float ControlHeight
        {
            get { return (float)GetValue(ControlHeightProperty); }
            set { SetValue(ControlHeightProperty, value); }
        }


        public static readonly DependencyProperty ControlHeightProperty =
            DependencyProperty.Register("ControlHeight", typeof(float),
              typeof(DataGridWithLabel), new PropertyMetadata(null));
        #endregion

        #region ClickCommand DP
        public RelayCommand ClickCommand
        {
            get { return (RelayCommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public static readonly DependencyProperty ClickCommandProperty =
            DependencyProperty.Register("ClickCommand", typeof(RelayCommand),
              typeof(DataGridWithLabel), new PropertyMetadata(null));
        #endregion

        #region ClickCommandParameter DP
        public object ClickCommandParameter
        {
            get { return (object)GetValue(ClickCommandParameterProperty); }
            set { SetValue(ClickCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty ClickCommandParameterProperty =
            DependencyProperty.Register("ClickCommandParameter", typeof(object),
              typeof(DataGridWithLabel), new PropertyMetadata(null));
        #endregion

    }
}
