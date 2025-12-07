namespace Library.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        T? GetById(int id);
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(int id);
        
        /// <summary>
        /// Find entities matching a predicate
        /// </summary>
        List<T> Find(Func<T, bool> predicate);
    }
}
