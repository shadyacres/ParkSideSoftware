using SwipeBox.Shared.Entities;
using System.Data.Entity;

namespace SwipeBox.DAL.Context
{
    public class SwipeBoxContext : DbContext, ISwipeBoxContext
    {
        public SwipeBoxContext()
            : base ("SwipeBoxContext")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Meeting> Meetings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
