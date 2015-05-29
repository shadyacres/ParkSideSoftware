// <copyright file="SwipeBoxContext.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>SwipeBox entity framework DB context</summary>

using SwipeBox.Shared.Entities;
using System.Data.Entity;

namespace SwipeBox.DAL.Context
{
    /// <summary>
    /// Swipebox entity framework DB context
    /// </summary>
    public class SwipeBoxContext : DbContext, ISwipeBoxContext
    {
        /// <summary>
        /// Initializes a new instance of the SwipeBoxContext class
        /// </summary>
        public SwipeBoxContext()
            : base ("SwipeBoxContext")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Gets or sets the Clients
        /// </summary>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Gets or sets companies
        /// </summary>
        public DbSet<Company> Companies { get; set; }

        /// <summary>
        /// Gets or sets the meetings
        /// </summary>
        public DbSet<Meeting> Meetings { get; set; }

        /// <summary>
        /// override of the base OnModelCreating method
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Execute the base method
            base.OnModelCreating(modelBuilder);
        }
    }
}
