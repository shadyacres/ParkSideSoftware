using System.Collections.Generic;

namespace SwipeBox.Shared.Entities
{
    public class Company
    {
        public int CompanyId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}
