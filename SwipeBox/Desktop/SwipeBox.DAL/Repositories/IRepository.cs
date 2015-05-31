using System.Linq;

namespace SwipeBox.DAL.Repositories
{
    /// <summary>
    /// Generic Repo interface
    /// </summary>
    /// <typeparam name="T">Type of entity the repo is for</typeparam>
    public interface IRepository<T> where T: class
    {
        /// <summary>
        /// Gets a Generic IQueryable of the entity
        /// </summary>
        IQueryable<T> Get { get; }

        /// <summary>
        /// Generic Delete method
        /// </summary>
        /// <param name="id">The entity's id</param>
        /// <returns>The deleted Entity</returns>
        T Delete(int id);

        /// <summary>
        /// Save an item
        /// </summary>
        /// <param name="itemToSave">The item to save</param>
        /// <returns>True if saved</returns>
        bool Save(T itemToSave);
    }
}
