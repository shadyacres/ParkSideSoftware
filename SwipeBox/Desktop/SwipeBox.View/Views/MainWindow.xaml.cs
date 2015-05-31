// <copyright file=".cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Interaction logic for mainwindow</summary>

using Microsoft.Practices.Unity;
using SwipeBox.UI.ViewModel;
using System.Windows;
namespace SwipeBox.UI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Set the viewmodel context to this
        /// </summary>
       [Dependency]
       public MainWindowViewModel ViewModel
        {
            set { DataContext = value; }
        }

        /// <summary>
        /// Initializes the MainWindow class
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
