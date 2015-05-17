using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwipeBox.Shared.Entities;
using SwipeBox.DAL.Repositories;

namespace SwipeBox.BusinessLogic
{
    public class ClientsBL : IClientsBL
    {
        private IClientRepository m_repo;

        #region Constructors
        public ClientsBL()
        {
            m_repo = new EFClientRepository();
        }

        public ClientsBL(IClientRepository repo)
        {
            m_repo = repo;
        }
        #endregion

        public string GetClientNameById(int id)
        {
            return m_repo.Get.FirstOrDefault(c => c.ClientId == id).Name;
        }


        public Client AddClient(string name, string email, string phoneNumber, string password)
        {
            var newClient = new Client
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Password = password,
                Active = true
            };

            m_repo.Save(newClient);
            return newClient;

        }

        public void DeleteClient(Client selectedClient)
        {
            selectedClient.Active = false;
            m_repo.Save(selectedClient);
        }

        public IEnumerable<Client> GetAllRegisteredClients()
        {
            return m_repo.Get.Where(c => c.Active);
        }

        public bool ClientExists(string name, string email, string phoneNumber)
        {
            return GetAllRegisteredClients().Any(p => p.Name == name &&
                                                 p.Email == email &&
                                                 p.PhoneNumber == phoneNumber);
        }

        public void UpdateClient(Client selectedClient)
        {
            m_repo.Save(selectedClient);
        }

        public void UpdateClient(Client selectedClient, string name, string email, string phoneNumber, string password)
        {
            selectedClient.Name = name;
            selectedClient.PhoneNumber = phoneNumber;
            selectedClient.Email = email;
            selectedClient.Password = password;

            UpdateClient(selectedClient);
        }

        public void GetClientMeetings(ref IEnumerable<Meeting> meetings)
        {
            foreach (var meeting in meetings)
            {
                meeting.Client = m_repo.Get.FirstOrDefault(c => c.ClientId == meeting.ClientId);
            }
        }

        public Client GetByDetails(string name, string phoneNumber, string email)
        {
            return m_repo.Get.FirstOrDefault(c => c.Name == name &&
                                             c.PhoneNumber == phoneNumber &&
                                             c.Email == email);
        }
    }
}
