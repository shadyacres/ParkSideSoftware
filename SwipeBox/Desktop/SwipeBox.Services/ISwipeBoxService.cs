// <copyright file="ISwipeBoxService.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Swipe box service contract</summary>

using SwipeBox.Services.DTO;
using System.Collections.Generic;
using System.ServiceModel;

namespace SwipeBox.Services
{
    /// <summary>
    /// Swipe box service contract
    /// </summary>
    [ServiceContract]
    public interface ISwipeBoxService
    {
        /// <summary>
        /// Add a meeting for a client for now
        /// </summary>
        /// <param name="clientId">the client id</param>
        /// <returns>whether or not the meeting was added</returns>
        [OperationContract]
        bool AddMeeting(int clientId);

        /// <summary>
        /// get a client from their email
        /// </summary>
        /// <param name="email">the client's email</param>
        /// <returns>The client</returns>
        [OperationContract]
        ClientDTO GetClientByEmail(string email);

        /// <summary>
        /// Gets all clients
        /// </summary>
        /// <returns>the list of clients</returns>
        [OperationContract]
        List<ClientDTO> GetAllClients();

        /// <summary>
        /// Authorize a user
        /// </summary>
        /// <param name="email">the user email</param>
        /// <param name="pass">the user password</param>
        /// <returns>whether or not the user has been authorized</returns>
        [OperationContract]
        bool AuthorizeUser(string email, string pass);
    }
}
