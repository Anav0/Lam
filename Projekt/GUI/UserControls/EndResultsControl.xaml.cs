﻿using System.Windows.Controls;
using Projekt.ViewModels;

namespace Projekt.GUI.UserControls
{
    /// <summary>
    ///     Interaction logic for EndResultsControl.xaml
    /// </summary>
    public partial class EndResultsControl : UserControl
    {
        public EndResultsControl(EndResultsViewModel viewmodel)
        {
            InitializeComponent();
            DataContext = viewmodel;
        }
    }
}