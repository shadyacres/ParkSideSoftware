using SwipeBox.Shared.Entities;
using System.Data.Entity;

namespace SwipeBox.DAL.Context
{
    public interface ISwipeBoxContext
    {
        DbSet<Client> Clients { get; set; }

        DbSet<Company> Companies { get; set; }

        DbSet<Meeting> Meetings { get; set; }

        void Dispose();

        int SaveChanges();
    }
}
