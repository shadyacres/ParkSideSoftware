using SwipeBox.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeBox.BusinessLogic
{
    public interface IClientsBL
    {
        Client AddClient(string name, string email, string phoneNumber, string password);
        void DeleteClient(Client selectedClient);
        IEnumerable<Client> GetAllRegisteredClients();
        string GetClientNameById(int id);
        bool ClientExists(string name, string email, string phoneNumber);
        void UpdateClient(Client SelectedClient);
        void UpdateClient(Client SelectedClient, string name, string email, string phoneNumber, string password);
        void GetClientMeetings(ref IEnumerable<Meeting> meetings);
        Client GetByDetails(string firstName, string lastName, string postcode);
    }
}