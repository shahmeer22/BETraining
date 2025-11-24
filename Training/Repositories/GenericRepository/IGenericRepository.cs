using System.Linq.Expressions;

namespace Training.Repositories.GenericRepository
{
    public interface IGenericRepository<T>
    {
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        T Update(T entity);
        Task<int> SaveChangesAsync();
    }
}
