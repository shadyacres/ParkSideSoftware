
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

namespace SwipeBox.ViewModel
{
    public class ClientsViewModel : BaseViewModel, IAutoRefresher
    {

        private const string ClientEnabledPropertyName = "ClientEnabled";
        private const string SelectedClientPropertyName = "SelectedClient";

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
                if (m_selectedClient != value)
                {
                    m_selectedClient = value;
                    OnNotifyPropertyChanged(SelectedClientPropertyName);
                    OnNotifyPropertyChanged(ClientEnabledPropertyName);
                }
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
            AddPatientView view = new AddPatientView();
            view.DataContext = new AddPatientViewModel(this);
            view.ShowDialog();
        }

        public RelayCommand DeleteClientCommand { get; private set; }
        public void DeleteClient(object parameter)
        {

        }

        public RelayCommand UpdateClientCommand { get; private set; }

        public void UpdateClient(object parameter)
        {

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
    }
}
