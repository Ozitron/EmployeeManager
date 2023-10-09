﻿using EmployeeManager.ViewModels;
using System.Windows;

namespace EmployeeManager.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();  
            DataContext = viewModel;
        }
    }
}
