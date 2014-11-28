using SwipeBox.Shared.Entities;
using System.Data.Entity;

namespace SwipeBox.DAL.Context
{
    class SwipeBoxContext : DbContext, ISwipeBoxContext
    {
        public SwipeBoxContext()
            : base ("SwipeBoxContext")
        {
           
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
