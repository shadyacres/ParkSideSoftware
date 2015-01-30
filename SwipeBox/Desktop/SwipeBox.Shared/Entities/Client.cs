/*
 * 
 * 
 * 
 * 
 * */

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SwipeBox.Shared.Entities
{
    /// <summary>
    /// Client entity
    /// </summary>
    [DataContract]
    public class Client
    {
        /// <summary>
        /// Client's ID
        /// </summary>
        [DataMember]
        public int ClientId { get; set; }

        /// <summary>
        /// Client's full name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Client's e-mail address
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// Client's contact number
        /// </summary>
        [DataMember]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Company the client works for
        /// </summary>
        [DataMember]
        public virtual Company Company { get; set; }

        /// <summary>
        /// Client's company ID
        /// </summary>
        [DataMember]
        public int? CompanyId { get; set; }

        /// <summary>
        /// List of meetings the client has attended
        /// </summary>
        [DataMember]
        public virtual ICollection<Meeting> Meetings { get; set; }

        /// <summary>
        /// Whether or not the client is currently active
        /// </summary>
        [DataMember]
        public bool Active { get; set; }
    }
}
