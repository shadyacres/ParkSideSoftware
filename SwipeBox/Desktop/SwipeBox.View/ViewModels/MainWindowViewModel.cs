// <copyright file="MainWindowViewModel.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>main window view interaction</summary>

namespace SwipeBox.UI.ViewModel
{
    /// <summary>
    /// main window view interaction
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        private const string CurrentViewModelPropertyName = "CurrentViewModel";

        private BaseViewModel m_currentVM;

        /// <summary>
        /// Initializes a new instance of the main window view model
        /// </summary>
        public MainWindowViewModel()
        {
            // initalize child views
            MenuVM = new MenuViewModel(this);
            CurrentViewModel = new HomeViewModel();
            
        }

        /// <summary>
        /// Gets or sets the current viewmodel
        /// </summary>
        public BaseViewModel CurrentViewModel
        {
            get
            {
                return m_currentVM;
            }

            set
            {
                //if (m_currentVM is IAutoRefresher)
                //{
                //    var refresher = m_currentVM as IAutoRefresher;
                //    refresher.Stop();
                //}

                m_currentVM = value;
                OnNotifyPropertyChanged(CurrentViewModelPropertyName);
            }
        }

        /// <summary>
        /// Gets the menum viewmodel
        /// </summary>
        public MenuViewModel MenuVM { get; private set; }
    }
}
