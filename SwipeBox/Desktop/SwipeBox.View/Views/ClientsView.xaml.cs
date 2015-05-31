// <copyright file="ClientsView.xaml.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Interaction logic for ClientsView.xaml</summary>

using System.Windows.Controls;

namespace SwipeBox.UI.View
{
    /// <summary>
    /// Interaction logic for ClientsView.xaml
    /// </summary>
    public partial class ClientsView : UserControl
    {
        /// <summary>
        /// Initialize the clientsview class
        /// </summary>
        public ClientsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle the selection changed event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event args</param>
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            passwordBox.Clear();
        }
    }
}
