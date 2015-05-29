// <copyright file="ISwipeBoxContext.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Interface for SwipeBoxContext interaction</summary>

using SwipeBox.Shared.Entities;
using System.Data.Entity;

namespace SwipeBox.DAL.Context
{
    /// <summary>
    /// Interface for SwipeBoxContext interaction
    /// </summary>
    public interface ISwipeBoxContext
    {
        /// <summary>
        /// Gets or sets clients
        /// </summary>
        DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Gets or sets the companies
        /// </summary>
        DbSet<Company> Companies { get; set; }

        /// <summary>
        /// Gets or sets the meetings
        /// </summary>
        DbSet<Meeting> Meetings { get; set; }

        /// <summary>
        /// Dispose the context
        /// </summary>
        void Dispose();

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns>1 if successful, 0 if not.</returns>
        int SaveChanges();
    }
}
