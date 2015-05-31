// <copyright file="ClientDTO.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Client Data transfer object</summary>

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SwipeBox.Services.DTO
{
    /// <summary>
    /// Client Data transfer object
    /// </summary>
    [DataContract]
    public class ClientDTO
    {
        /// <summary>
        /// Gets or sets the client id
        /// </summary>
        [DataMember]
        public int ClientID { get; set; }

        /// <summary>
        /// Gets or sets the client name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the client's email
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the client phone number
        /// </summary>
        [DataMember]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the client's meetings
        /// </summary>
        [IgnoreDataMember]
        public IList<MeetingDTO> Meetings { get; set; }
    }
}