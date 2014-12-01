using System.Linq;

namespace SwipeBox.DAL.Repositories
{
    public interface IRepository<T> where T: class
    {
        IQueryable<T> Get { get; }

        T Delete(int id);

        bool Save(T itemToSave);
    }
}
