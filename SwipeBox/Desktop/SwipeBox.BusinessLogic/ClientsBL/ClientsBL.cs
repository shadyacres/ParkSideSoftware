// <copyright file="ClientsBL.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Clients business logic</summary>

using SwipeBox.DAL.Repositories;
using SwipeBox.Shared.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SwipeBox.BusinessLogic
{
    /// <summary>
    /// Clients business logic
    /// </summary>
    public class ClientsBL : IClientsBL
    {
        private IClientRepository m_repo;

        #region Constructors
        /// <summary>
        /// Initializes a new instance fo the ClientsBL class - default repository
        /// </summary>
        public ClientsBL()
        {
            m_repo = new EFClientRepository();
        }

        /// <summary>
        /// Initializes a new isntance of the ClientsBL class - passed repository
        /// </summary>
        /// <param name="repo">client's repository</param>
        public ClientsBL(IClientRepository repo)
        {
            m_repo = repo;
        }
        #endregion

        /// <summary>
        /// Gets a client from their id number
        /// </summary>
        /// <param name="id">The customers id</param>
        /// <returns>the client name</returns>
        public string GetClientNameById(int id)
        {
            return m_repo.Get.FirstOrDefault(c => c.ClientId == id).Name;
        }

        /// <summary>
        ///  Adds a new client from parameters
        /// </summary>
        /// <param name="name">Client's name</param>
        /// <param name="email">Clients email</param>
        /// <param name="phoneNumber">The phone number</param>
        /// <param name="password">the password</param>
        /// <returns>The new client</returns>
        public Client AddClient(string name, string email, string phoneNumber, string password)
        {
            // Create a client
            var newClient = new Client
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Password = password,
                Active = true
            };

            // Save and return
            m_repo.Save(newClient);
            return newClient;

        }

        /// <summary>
        /// Delete a selected client
        /// </summary>
        /// <param name="selectedClient">Client to delete</param>
        public void DeleteClient(Client selectedClient)
        {
            // Mark as inactive and save
            selectedClient.Active = false;
            m_repo.Save(selectedClient);
        }

        /// <summary>
        /// Gets all active clients
        /// </summary>
        /// <returns>IEnumberable of active clients</returns>
        public IEnumerable<Client> GetAllRegisteredClients()
        {
            return m_repo.Get.Where(c => c.Active);
        }

        /// <summary>
        /// Gets a value indicating that a client exists
        /// </summary>
        /// <param name="name">Client name</param>
        /// <param name="email">client email</param>
        /// <param name="phoneNumber">client's phone number</param>
        /// <returns>True if the client exists, false if not</returns>
        public bool ClientExists(string name, string email, string phoneNumber)
        {
            return GetAllRegisteredClients().Any(p => p.Name == name &&
                                                 p.Email == email &&
                                                 p.PhoneNumber == phoneNumber);
        }

        /// <summary>
        /// Updates a client 
        /// </summary>
        /// <param name="selectedClient">Client to update</param>
        public void UpdateClient(Client selectedClient)
        {
            m_repo.Save(selectedClient);
        }

        /// <summary>
        /// Updates a client
        /// </summary>
        /// <param name="selectedClient">Client to update</param>
        /// <param name="name">client's updated name</param>
        /// <param name="email">clients updated email</param>
        /// <param name="phoneNumber">client's updated phone number</param>
        /// <param name="password">client's updated password</param>
        public void UpdateClient(Client selectedClient, string name, string email, string phoneNumber, string password)
        {
            selectedClient.Name = name;
            selectedClient.PhoneNumber = phoneNumber;
            selectedClient.Email = email;
            selectedClient.Password = password;

            UpdateClient(selectedClient);
        }

        /// <summary>
        /// Gets a list of clients
        /// </summary>
        /// <param name="meetings">Meeting list</param>
        public void GetClientMeetings(ref IEnumerable<Meeting> meetings)
        {
            foreach (var meeting in meetings)
            {
                meeting.Client = m_repo.Get.FirstOrDefault(c => c.ClientId == meeting.ClientId);
            }
        }
        
        /// <summary>
        /// Get a client from their details
        /// </summary>
        /// <param name="name">clients name</param>
        /// <param name="phoneNumber">client's phone number</param>
        /// <param name="email">client's email</param>
        /// <returns>The client that was found</returns>
        public Client GetByDetails(string name, string phoneNumber, string email)
        {
            // Finds the first client or a null if no client matches
            return m_repo.Get.FirstOrDefault(c => c.Name == name &&
                                             c.PhoneNumber == phoneNumber &&
                                             c.Email == email);
        }
    }
}
