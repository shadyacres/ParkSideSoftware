// <copyright file="Configuration.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Configures EF code first migrations</summary>

namespace SwipeBox.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Configures EF code first migrations
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<SwipeBox.DAL.Context.SwipeBoxContext>
    {
        /// <summary>
        /// Migration configuration
        /// </summary>
        public Configuration()
        {
            // Enable automatic migrations
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "SwipeBox.DAL.Context.SwipeBoxContext";
        }

        /// <summary>
        /// Seed method - Test only
        /// </summary>
        /// <param name="context">the db context</param>
        //protected override void Seed(SwipeBox.DAL.Context.SwipeBoxContext context)
        //{
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        //}
    }
}
