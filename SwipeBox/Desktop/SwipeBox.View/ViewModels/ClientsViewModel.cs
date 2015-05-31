// <copyright file="ClientsViewModel.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Clients View Model - ui interfaction</summary>

using SwipeBox.BusinessLogic;
using SwipeBox.Shared;
using SwipeBox.Shared.Constants;
using SwipeBox.Shared.Entities;
using SwipeBox.Shared.Interface;
using SwipeBox.UI.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Controls;

namespace SwipeBox.UI.ViewModel
{
    /// <summary>
    /// Clients view model UI interfaction
    /// </summary>
    public class ClientsViewModel : BaseViewModel, IAutoRefresher
    {

        private const string ClientEnabledPropertyName = "ClientEnabled";
        private const string SelectedClientPropertyName = "SelectedClient";
        private const string NamePropertyName = "Name";
        private const string PhoneNumberPropertyName = "PhoneNumber";
        private const string EmailPropertyName = "Email";
        private const string PasswordPropertyName = "Password";

        private string m_name;
        private string m_email;
        private string m_phoneNumber;
        private string m_password;
        private IClientsBL m_clientsBL;
        private Client m_selectedClient;
        private Timer m_refreshTimer;

        #region Constructors
        /// <summary>
        /// Iniitializes a new instance of the ClientsViewModel class
        /// </summary>
        /// <param name="clientsBL">Clients Business logic</param>
        public ClientsViewModel(IClientsBL clientsBL)
        {
            // initialize variables and commands
            m_clientsBL = clientsBL;
            AddClientCommand = new RelayCommand(AddClient);
            DeleteClientCommand = new RelayCommand(DeleteClient, p => CanModifyClient);
            UpdateClientCommand = new RelayCommand(UpdateClient, p => CanModifyClient);
            Clients = new ObservableCollection<Client>();

            // start page refreshing
            m_refreshTimer = new Timer(Refresh, null, GlobalConstants.NoRefreshInterval, GlobalConstants.NoRefreshInterval);
            RefreshClients();
            StartTimer();
        }
        #endregion

        /// <summary>
        /// Start refresh timer
        /// </summary>
        public void StartTimer()
        {
            m_refreshTimer.Change(GlobalConstants.RefreshInterval, GlobalConstants.RefreshInterval);
        }

        /// <summary>
        /// Refresh clients list
        /// </summary>
        public void RefreshClients()
        {
#if DEBUG
            // IF debugging, time how long it takes
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
#endif
            // Check main app thread isn#t null
            if (System.Windows.Application.Current != null)
            {
                // invoke action on application thread - Must update UI values on this thread
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => 
                {
                    var selected = SelectedClient;
                    Clients.Clear();

                    // Clear clients and re-add
                    foreach (var client in m_clientsBL.GetAllRegisteredClients().OrderBy(c => c.Name))
                    {
                        Clients.Add(client);
                    }

                    // re select the selected client
                    if (selected != null)
                    {
                        SelectedClient = Clients.FirstOrDefault(c => c.ClientId == selected.ClientId);
                    }
                }));
            }
#if DEBUG
            // if debugging, show time taken
            sw.Stop();
            System.Diagnostics.Debug.WriteLine("Refresh Client List Time: " + sw.ElapsedMilliseconds + "ms");
#endif
        }

        /// <summary>
        /// Gets or sets observable collection of clients
        /// </summary>
        public ObservableCollection<Client> Clients { get; set; }

        /// <summary>
        /// Gets a value indication whether the selected client is not null
        /// </summary>
        public bool ClientEnabled
        {
            get
            {
                return SelectedClient != null;
            }
        }

        /// <summary>
        /// Gets or sets the current client
        /// </summary>
        public Client SelectedClient
        {
            get
            {
                return m_selectedClient;
            }
            set
            {
                // Check for null values
                if (value != null && m_selectedClient != value)
                {
                    if ((m_selectedClient != null && m_selectedClient.ClientId != value.ClientId) || m_selectedClient == null)
                    {
                        // Update other values
                        Name = value.Name;
                        PhoneNumber = value.PhoneNumber;
                        Email = value.Email;
                        Password = value.Password;
                    }

                    m_selectedClient = value;
                    OnNotifyPropertyChanged(SelectedClientPropertyName);
                    OnNotifyPropertyChanged(ClientEnabledPropertyName);
                }
            }
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }

            set
            {
                m_name = value;
                OnNotifyPropertyChanged(NamePropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email
        {
            get
            {
                return m_email;
            }

            set
            {
                m_email = value;
                OnNotifyPropertyChanged(EmailPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the phone number
        /// </summary>
        public string PhoneNumber
        {
            get
            {
                return m_phoneNumber;
            }

            set
            {
                m_phoneNumber = value;
                OnNotifyPropertyChanged(PhoneNumberPropertyName);
            }
        }

        /// <summary>
        ///  Gets or sets the password
        /// </summary>
        public string Password
        {
            get
            {
                return m_password;
            }

            set
            {
                m_password = value;
                OnNotifyPropertyChanged(PasswordPropertyName);
            }
        }

        /// <summary>
        /// Gets a value indicating whether a client is currently selected
        /// </summary>
        private bool CanModifyClient
        {
            get
            {
                return SelectedClient != null;
            }
        }

        #region Commands
        /// <summary>
        /// Gets the Add client command
        /// </summary>
        public RelayCommand AddClientCommand { get; private set; }

        /// <summary>
        /// Executes the add client command
        /// </summary>
        /// <param name="parameter">binding parameter</param>
        public void AddClient(object parameter)
        {
            // initializes an addClientsView with this as its parent
            AddClientView view = new AddClientView();
            view.DataContext = new AddClientViewModel(this);
            view.ShowDialog();
        }

        /// <summary>
        /// Gets the delete command
        /// </summary>
        public RelayCommand DeleteClientCommand { get; private set; }

        /// <summary>
        /// Executes the delete client command
        /// </summary>
        /// <param name="parameter">binding parameter</param>
        public void DeleteClient(object parameter)
        {
            // Warn user that the action can't be undone
            if (System.Windows.MessageBoxResult.OK == System.Windows.MessageBox.Show(Properties.Resources.DeleteMessage,
                                                                                    Properties.Resources.DeleteCaption,
                                                                                    System.Windows.MessageBoxButton.OKCancel,
                                                                                    System.Windows.MessageBoxImage.Warning))
            {
                // run the delete client function
                m_clientsBL.DeleteClient(SelectedClient);

                // clear the selected client as it has now been deleted
                SelectedClient = null;
                ClearValues();
            }
        }

        /// <summary>
        /// Gets the updateclient command
        /// </summary>
        public RelayCommand UpdateClientCommand { get; private set; }

        /// <summary>
        ///  Update client command
        /// </summary>
        /// <param name="parameter">binding parameter</param>
        public void UpdateClient(object parameter)
        {
            // Gets the password and update the client
            var password = (parameter as PasswordBox).Password;
            m_clientsBL.UpdateClient(SelectedClient, Name, Email, PhoneNumber, password);
        }
        #endregion

        /// <summary>
        /// Refresh action
        /// </summary>
        /// <param name="state">refresh state</param>
        public void Refresh(object state)
        {
            RefreshClients();
        }

        /// <summary>
        /// Stop refreshing
        /// </summary>
        public void Stop()
        {
            m_refreshTimer.Change(GlobalConstants.NoRefreshInterval, GlobalConstants.NoRefreshInterval);
        }

        /// <summary>
        ///  Clear all values.
        /// </summary>
        internal void ClearValues()
        {
            SelectedClient = null;
        }
    }
}
