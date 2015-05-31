// <copyright file="ICLientsBL.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Interface for Clients business logic</summary>

using SwipeBox.Shared.Entities;
using System.Collections.Generic;

namespace SwipeBox.BusinessLogic
{
    /// <summary>
    /// Interface for clients business logic
    /// </summary>
    public interface IClientsBL
    {
        /// <summary>
        ///  Adds a new client from parameters
        /// </summary>
        /// <param name="name">Client's name</param>
        /// <param name="email">Clients email</param>
        /// <param name="phoneNumber">The phone number</param>
        /// <param name="password">the password</param>
        /// <returns>The new client</returns>
        Client AddClient(string name, string email, string phoneNumber, string password);

        /// <summary>
        /// Delete a selected client
        /// </summary>
        /// <param name="selectedClient">Client to delete</param>
        void DeleteClient(Client selectedClient);

        /// <summary>
        /// Gets all active clients
        /// </summary>
        /// <returns>IEnumberable of active clients</returns>
        IEnumerable<Client> GetAllRegisteredClients();

        /// <summary>
        /// Gets a client from their id number
        /// </summary>
        /// <param name="id">The customers id</param>
        /// <returns>the client name</returns>
        string GetClientNameById(int id);

        /// <summary>
        /// Gets a value indicating that a client exists
        /// </summary>
        /// <param name="name">Client name</param>
        /// <param name="email">client email</param>
        /// <param name="phoneNumber">client's phone number</param>
        /// <returns>True if the client exists, false if not</returns>
        bool ClientExists(string name, string email, string phoneNumber);

        /// <summary>
        /// Updates a client 
        /// </summary>
        /// <param name="selectedClient">Client to update</param>
        void UpdateClient(Client SelectedClient);

        /// <summary>
        /// Updates a client
        /// </summary>
        /// <param name="selectedClient">Client to update</param>
        /// <param name="name">client's updated name</param>
        /// <param name="email">clients updated email</param>
        /// <param name="phoneNumber">client's updated phone number</param>
        /// <param name="password">client's updated password</param>
        void UpdateClient(Client SelectedClient, string name, string email, string phoneNumber, string password);

        /// <summary>
        /// Gets a list of clients
        /// </summary>
        /// <param name="meetings">Meeting list</param>
        void GetClientMeetings(ref IEnumerable<Meeting> meetings);

        /// <summary>
        /// Get a client from their details
        /// </summary>
        /// <param name="name">clients name</param>
        /// <param name="phoneNumber">client's phone number</param>
        /// <param name="email">client's email</param>
        /// <returns>The client that was found</returns>
        Client GetByDetails(string firstName, string lastName, string postcode);
    }
}