using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SwipeBox.Services.DTO
{
    [DataContract]
    public class ClientDTO
    {
        [DataMember]
        public int ClientID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [IgnoreDataMember]
        public IList<MeetingDTO> Meetings { get; set; }
    }
}