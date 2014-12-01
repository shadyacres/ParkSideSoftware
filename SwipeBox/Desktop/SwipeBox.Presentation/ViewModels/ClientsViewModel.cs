
using SwipeBox.Shared.Entities;
using System.Collections.Generic;
namespace SwipeBox.Presentation
{
    public class ClientsViewModel : BaseViewModel
    {
        public List<Client> Clients
        { 
            get
            {
                return new List<Client>
                {
                    new Client { Name = "Stan Marsh", Email="stan.marsh@gmail.com", PhoneNumber="01242543562" },
                    new Client { Name = "Kyle Brofloski", Email="kyle.brofloski@gmail.com", PhoneNumber="01242543563" },
                    new Client { Name = "Eric Cartman", Email="eric.cartman@gmail.com", PhoneNumber="01242543564" },
                    new Client { Name = "Kenny McCormick", Email="kenny.mccormick@gmail.com", PhoneNumber="01242543565" },
                };
            }
        }
    }
}
