// <copyright file="AddClientView.xaml.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Interaction logic for AddClientView.xaml</summary>

using System.Windows;

namespace SwipeBox.UI.View
{
    /// <summary>
    /// Interaction logic for AddClientView.xaml
    /// </summary>
    public partial class AddClientView : Window
    {
        public AddClientView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Execute the save button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event args</param>
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Execute the window loaded event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event args</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.firstNameTxtBox.Focus();
        }
    }
}
