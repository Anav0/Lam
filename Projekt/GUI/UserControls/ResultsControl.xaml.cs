﻿using System.Windows.Controls;
using Projekt.ViewModels;

namespace Projekt
{
    /// <summary>
    ///     Interaction logic for Results.xaml
    /// </summary>
    public partial class ResultsControl : UserControl
    {
        public ResultsControl(ResultsViewModel viewmodel)
        {
            InitializeComponent();
            DataContext = viewmodel;
        }
    }
}