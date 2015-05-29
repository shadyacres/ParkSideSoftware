// <copyright file="IClientRepository.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Interface for client repo interaction</summary>

using SwipeBox.Shared.Entities;

namespace SwipeBox.DAL.Repositories
{
    /// <summary>
    /// Interface for client repo interaction
    /// </summary>
    public interface IClientRepository : IRepository<Client>
    {
        /// <summary>
        /// Get client by email
        /// </summary>
        /// <param name="email">the client's email</param>
        /// <returns>the client object</returns>
        Client GetByEmail(string email);

        /// <summary>
        /// Authorize a user via their email address.
        /// </summary>
        /// <param name="email">The clients email</param>
        /// <param name="pass">The client's password</param>
        /// <returns>True if authorized, false if not.</returns>
        bool AuthorizeUser(string email, string pass);
    }
}
