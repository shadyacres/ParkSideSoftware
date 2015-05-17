using SwipeBox.Shared.Entities;

namespace SwipeBox.DAL.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
        Client GetByEmail(string email);

        bool AuthorizeUser(string email, string pass);
    }
}
