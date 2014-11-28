using SwipeBox.DAL.Context;
using SwipeBox.Shared.Entities;
using System;
using System.Linq;

namespace SwipeBox.DAL.Repositories
{
    public class EFClientRepository : IClientRepository, IDisposable
    {
        private ISwipeBoxContext m_context;

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

            if (clientToSave.ClientId == 0)
            {
                m_context.Clients.Add(clientToSave);
            }
            else
            {
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
                    retVal = false;
                }
            }

            m_context.SaveChanges();
            return retVal;
        }

        public void Dispose()
        {
            m_context.Dispose();
        }
    }
}
