// <copyright file=".cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary></summary>

using SwipeBox.BusinessLogic;
using SwipeBox.Shared;
namespace SwipeBox.UI.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        MainWindowViewModel m_parent;

        public MenuViewModel(MainWindowViewModel parent)
        {
            // Set parent and commands
            m_parent = parent;

            // Set naviation commands
            NavigateToHomeCommand = new RelayCommand(NavigateToHome);
            NavigateToClientsCommand = new RelayCommand(NavigateToClients);
            NavigateToSettingsCommand = new RelayCommand(NavigateToSettings);
        }

        /// <summary>
        /// Gets the Navigate to home command
        /// </summary>
        public RelayCommand NavigateToHomeCommand { get; private set; }

        /// <summary>
        /// Navigate to home page
        /// </summary>
        /// <param name="binding">binding parameter</param>
        private void NavigateToHome(object binding)
        {
            m_parent.CurrentViewModel = new HomeViewModel();
        }

        /// <summary>
        /// Gets the Navigate to clients page command
        /// </summary>
        public RelayCommand NavigateToClientsCommand { get; private set; }

        /// <summary>
        /// Navigate to client page
        /// </summary>
        /// <param name="binding">binding parameter</param>
        private void NavigateToClients(object binding)
        {
            m_parent.CurrentViewModel = new ClientsViewModel(new ClientsBL());
        }

        /// <summary>
        /// Gets the navigate to settings command
        /// </summary>
        public RelayCommand NavigateToSettingsCommand { get; private set; }

        /// <summary>
        /// Navigate to settings pages
        /// </summary>
        /// <param name="obj">binding param</param>
        private void NavigateToSettings(object obj)
        {
            m_parent.CurrentViewModel = new SettingsViewModel();
        }
    }
}