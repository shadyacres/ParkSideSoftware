using SwipeBox.BusinessLogic;
using SwipeBox.Shared;
using SwipeBox.UI.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeBox.UI.ViewModel
{
    public class AddClientViewModel : BaseViewModel
    {
        private const string NamePropertyName = "Name";
        private const string PhoneNumberPropertyName = "PhoneNumber";
        private const string EmailPropertyName = "Email";

        private string m_name;
        private string m_email;
        private string m_phoneNumber;
        private IClientsBL m_clientBl;
        private ClientsViewModel m_parent;

        public AddClientViewModel(ClientsViewModel parent)
        {
            m_parent = parent;
            m_clientBl = new ClientsBL();
            SaveCommand = new RelayCommand(Save, p => CanSave);
            CancelCommand = new RelayCommand(Close);
        }

        public void ClearValues()
        {
            Name = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
        }

        public RelayCommand CancelCommand { get; private set; }

        private void Close(object obj)
        {
            if (obj is AddClientView)
            {
                var window = obj as AddClientView;
                window.Close();
            }
        }

        public RelayCommand SaveCommand { get; private set; }

        public bool CanSave
        {
            get
            {
                return !string.IsNullOrEmpty(Name) &&
                       !string.IsNullOrEmpty(Email) &&
                       !string.IsNullOrEmpty(PhoneNumber);
            }
        }

        private void Save(object obj)
        {
            // Check if existing
            if (m_clientBl.ClientExists(Name, Email, PhoneNumber))
            {
                if (System.Windows.MessageBoxResult.Yes == System.Windows.MessageBox.Show(Properties.Resources.ClientExistsMessage,
                                                                                          Properties.Resources.ClientExistsCaption,
                                                                                          System.Windows.MessageBoxButton.YesNo,
                                                                                          System.Windows.MessageBoxImage.Error))
                {
                    ClearValues();
                }
                else
                {
                    Close(obj);
                }
            }
            else
            {
                m_parent.ClearValues();
                m_parent.Stop();
                m_parent.SelectedClient = m_clientBl.AddClient(Name, Email, PhoneNumber);
                m_parent.StartTimer();
                Close(obj);
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

    }
}

