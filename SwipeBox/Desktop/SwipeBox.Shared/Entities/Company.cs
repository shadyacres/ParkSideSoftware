// <copyright file="Company.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Company entity</summary>

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SwipeBox.Shared.Entities
{
    /// <summary>
    /// Company entity
    /// </summary>
    public class Company
    {
        /// <summary>
        /// Gets or sets the company id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or set teh company name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or set the company#s clients
        /// </summary>
        public virtual ICollection<Client> Clients { get; set; }
    }
}
