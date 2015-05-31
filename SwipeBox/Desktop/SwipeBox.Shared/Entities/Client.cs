// <copyright file="Client.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Client Entity</summary>

using System.Collections.Generic;
using System.Runtime.Serialization;

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
        /// Gets or sets client's password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets password salt
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// Company the client works for
        /// </summary>
        public virtual Company Company { get; set; }

        /// <summary>
        /// Client's company ID
        /// </summary>
        public int? CompanyId { get; set; }

        /// <summary>
        /// List of meetings the client has attended
        /// </summary>
        public virtual ICollection<Meeting> Meetings { get; set; }

        /// <summary>
        /// Whether or not the client is currently active
        /// </summary>
        public bool Active { get; set; }
    }
}
