// <copyright file="AddClientViewModel.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Add Client View model</summary>

using SwipeBox.BusinessLogic;
using SwipeBox.Shared;
using SwipeBox.UI.View;
using System.Windows.Controls;

namespace SwipeBox.UI.ViewModel
{
    /// <summary>
    /// Add client view model
    /// </summary>
    public class AddClientViewModel : BaseViewModel
    {
        private const string NamePropertyName = "Name";
        private const string PhoneNumberPropertyName = "PhoneNumber";
        private const string EmailPropertyName = "Email";
        private const string PasswordPropertyName = "Password";

        private string m_name;
        private string m_email;
        private string m_phoneNumber;
        private string m_password;
        private IClientsBL m_clientBl;
        private ClientsViewModel m_parent;

        /// <summary>
        /// Initialzies a new instance of the AddClientViewModel class
        /// </summary>
        /// <param name="parent">The parent window</param>
        public AddClientViewModel(ClientsViewModel parent)
        {
            m_parent = parent;
            m_clientBl = new ClientsBL();
            SaveCommand = new RelayCommand(Save, p => CanSave);
            CancelCommand = new RelayCommand(Close);
        }

        /// <summary>
        /// Clear all Client values
        /// </summary>
        public void ClearValues()
        {
            Name = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Password = string.Empty;
        }

        /// <summary>
        /// Gets the cancel command
        /// </summary>
        public RelayCommand CancelCommand { get; private set; }

        /// <summary>
        /// Close action
        /// </summary>
        /// <param name="obj">binding param - The window</param>
        private void Close(object obj)
        {
            // ensure correct type and close
            if (obj is AddClientView)
            {
                var window = obj as AddClientView;
                window.Close();
            }
        }

        /// <summary>
        /// Gets the Save command
        /// </summary>
        public RelayCommand SaveCommand { get; private set; }

        /// <summary>
        /// Gets a value indacting whether the user can save
        /// </summary>
        public bool CanSave
        {
            get
            {
                return !string.IsNullOrEmpty(Name) &&
                       !string.IsNullOrEmpty(Email) &&
                       !string.IsNullOrEmpty(PhoneNumber) &&
                       !string.IsNullOrEmpty(Password);
            }
        }

        /// <summary>
        /// Execute save action
        /// </summary>
        /// <param name="obj">binding parameter - the window</param>
        private void Save(object obj)
        {
            // Check if existing
            if (m_clientBl.ClientExists(Name, Email, PhoneNumber))
            {
                // If client exists - warn user and clear
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
                // Set client parameters and add a client for it.
                var passWord = (obj as PasswordBox).Password;
                m_parent.ClearValues();
                m_parent.Stop();
                m_parent.SelectedClient = m_clientBl.AddClient(Name, Email, PhoneNumber, passWord);
                m_parent.StartTimer();
                
            }
        }

        /// <summary>
        /// Gets or sets the client name
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
        /// Gets or sets the email value
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
        /// Gets or sets the phone number value
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
        /// Gets or sets the password value
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
    }
}

