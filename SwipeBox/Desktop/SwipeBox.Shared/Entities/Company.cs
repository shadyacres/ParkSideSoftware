using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SwipeBox.Shared.Entities
{
    [DataContract]
    public class Company
    {
        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public virtual ICollection<Client> Clients { get; set; }
    }
}
