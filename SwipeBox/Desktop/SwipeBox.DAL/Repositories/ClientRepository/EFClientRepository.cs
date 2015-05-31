// <copyright file="EFClientRepository.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Client repo implemented in EF</summary>

using SwipeBox.DAL.Context;
using SwipeBox.Shared.Entities;
using System;
using System.Linq;

namespace SwipeBox.DAL.Repositories
{
    /// <summary>
    /// Client repo implemented in EF
    /// </summary>
    public class EFClientRepository : IClientRepository, IDisposable
    {
        private ISwipeBoxContext m_context;

        /// <summary>
        /// Initializes a new instance of the EFClientRepository with the default context
        /// </summary>
        public EFClientRepository()
        {
            m_context = new SwipeBoxContext();
        }

        /// <summary>
        /// Initializes a new instance of the EFClientRepository with a custom context
        /// </summary>
        /// <param name="context">custom swipebox context</param>
        public EFClientRepository(ISwipeBoxContext context)
        {
            m_context = context;
        }

        /// <summary>
        /// Gets a list of all clients as an IQueryable for further sorting and filtering
        /// </summary>
        public IQueryable<Client> Get
        {
            get 
            { 
                return m_context.Clients; 
            }
        }

        /// <summary>
        /// Delete a client from the database
        /// </summary>
        /// <param name="clientId">The client's Id to delete</param>
        /// <returns>the deleted client</returns>
        public Client Delete(int clientId)
        {
            var client = m_context.Clients.Find(clientId);
            if (client != null)
            {
                // Remove the found client from the context.
                m_context.Clients.Remove(client);
                m_context.SaveChanges();
            }

            return client;
        }

        /// <summary>
        /// Save a client to the data context - adding a new client or updating an existing one.
        /// </summary>
        /// <param name="clientToSave">The client to save</param>
        public bool Save(Client clientToSave)
        {
            var retVal = true;

            // Client doesn't exit - INSERT
            if (clientToSave.ClientId == 0)
            {
                m_context.Clients.Add(clientToSave);
            }
            else
            {
                // Client exists - UPDATE
                var clientEntry = m_context.Clients.Find(clientToSave.ClientId);
                if (clientEntry != null)
                {
                    clientEntry.Name = clientToSave.Name;
                    clientEntry.PhoneNumber = clientToSave.PhoneNumber;
                    clientEntry.Email = clientToSave.Email;
                    clientEntry.Company = clientToSave.Company;
                }
                else
                {
                    // customer not found - not saved.
                    retVal = false;
                }
            }

            m_context.SaveChanges();
            return retVal;
        }

        /// <summary>
        /// Get Clients by email
        /// </summary>
        /// <param name="email">The client's email</param>
        /// <returns>The client object</returns>
        public Client GetByEmail(string email)
        {
            return m_context.Clients.FirstOrDefault(c => c.Email == email && c.Active);
        }

        /// <summary>
        /// Authorize a user by their email and password
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="pass">user password</param>
        /// <returns>true if the user is authorized, false if not.</returns>
        public bool AuthorizeUser(string email, string pass)
        {
            var retVal = false;
            var client = m_context.Clients.FirstOrDefault(c => c.Email == email);
            if (client == null)
            {
                retVal = false;
            }
            else
            {
                if (client.Password == pass)
                {
                    retVal = true;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Dispose the underlying context.
        /// </summary>
        public void Dispose()
        {
            m_context.Dispose();
        }
    }
}
