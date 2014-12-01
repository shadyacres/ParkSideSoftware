/*
 * 
 * 
 * 
 * 
 * */

using System.Collections.Generic;

namespace SwipeBox.Shared.Entities
{
    /// <summary>
    /// Client entity
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Client's ID
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Client's full name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Client's e-mail address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Client's contact number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Company the client works for
        /// </summary>
        public virtual Company Company { get; set; }

        /// <summary>
        /// Client's company ID
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// List of meetings the client has attended
        /// </summary>
        public virtual ICollection<Meeting> Meetings { get; set; }
    }
}
