
using SwipeBox.Shared;
using SwipeBox.Shared.Constants;
using SwipeBox.Shared.Entities;
using SwipeBox.Shared.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Linq;
using SwipeBox.BusinessLogic;
using System;
using SwipeBox.UI;
using SwipeBox.UI.View;

namespace SwipeBox.UI.ViewModel
{
    public class ClientsViewModel : BaseViewModel, IAutoRefresher
    {

        private const string ClientEnabledPropertyName = "ClientEnabled";
        private const string SelectedClientPropertyName = "SelectedClient";
        private const string NamePropertyName = "Name";
        private const string PhoneNumberPropertyName = "PhoneNumber";
        private const string EmailPropertyName = "Email";

        private string m_name;
        private string m_email;
        private string m_phoneNumber;
        private IClientsBL m_clientsBL;
        private Client m_selectedClient;
        private Timer m_refreshTimer;

        #region Constructors
        public ClientsViewModel(IClientsBL clientsBL)
        {
            m_clientsBL = clientsBL;
            AddClientCommand = new RelayCommand(AddClient);
            DeleteClientCommand = new RelayCommand(DeleteClient, p => CanModifyClient);
            UpdateClientCommand = new RelayCommand(UpdateClient, p => CanModifyClient);
            Clients = new ObservableCollection<Client>();

            m_refreshTimer = new Timer(Refresh, null, GlobalConstants.NoRefreshInterval, GlobalConstants.NoRefreshInterval);
            RefreshClients();
            StartTimer();
        }
        #endregion

        public void StartTimer()
        {
            m_refreshTimer.Change(GlobalConstants.RefreshInterval, GlobalConstants.RefreshInterval);
        }

        public void RefreshClients()
        {
#if DEBUG
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
#endif
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => 
                {
                    var selected = SelectedClient;
                    Clients.Clear();

                    foreach (var client in m_clientsBL.GetAllRegisteredClients().OrderBy(c => c.Name))
                    {
                        Clients.Add(client);
                    }

                    if (selected != null)
                    {
                        SelectedClient = Clients.FirstOrDefault(c => c.ClientId == selected.ClientId);
                    }
                }));
            }
#if DEBUG
            sw.Stop();
            System.Diagnostics.Debug.WriteLine("Refresh Client List Time: " + sw.ElapsedMilliseconds + "ms");
#endif
        }

        public ObservableCollection<Client> Clients { get; set; }

        public bool ClientEnabled
        {
            get
            {
                return SelectedClient != null;
            }
        }

        public Client SelectedClient
        {
            get
            {
                return m_selectedClient;
            }
            set
            {
                if (value != null && m_selectedClient != value)
                {
                    if ((m_selectedClient != null && m_selectedClient.ClientId != value.ClientId) || m_selectedClient == null)
                    {
                        Name = value.Name;
                        PhoneNumber = value.PhoneNumber;
                        Email = value.Email;
                    }

                    m_selectedClient = value;
                    OnNotifyPropertyChanged(SelectedClientPropertyName);
                    OnNotifyPropertyChanged(ClientEnabledPropertyName);
                }
            }
        }

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

        private bool CanModifyClient
        {
            get
            {
                return SelectedClient != null;
            }
        }

        #region Commands
        public RelayCommand AddClientCommand { get; private set; }

        public void AddClient(object parameter)
        {
            AddClientView view = new AddClientView();
            view.DataContext = new AddClientViewModel(this);
            view.ShowDialog();
        }

        public RelayCommand DeleteClientCommand { get; private set; }
        public void DeleteClient(object parameter)
        {
            if (System.Windows.MessageBoxResult.OK == System.Windows.MessageBox.Show(Properties.Resources.DeleteMessage,
                                                                                    Properties.Resources.DeleteCaption,
                                                                                    System.Windows.MessageBoxButton.OKCancel,
                                                                                    System.Windows.MessageBoxImage.Warning))
            {
                m_clientsBL.DeleteClient(SelectedClient);
                SelectedClient = null;
                ClearValues();
            }
        }

        public RelayCommand UpdateClientCommand { get; private set; }

        public void UpdateClient(object parameter)
        {
            m_clientsBL.UpdateClient(SelectedClient, Name, Email, PhoneNumber);
        }
        #endregion

        public void Refresh(object state)
        {
            RefreshClients();
        }

        public void Stop()
        {
            m_refreshTimer.Change(GlobalConstants.NoRefreshInterval, GlobalConstants.NoRefreshInterval);
        }

        internal void ClearValues()
        {
            SelectedClient = null;
        }
    }
}
