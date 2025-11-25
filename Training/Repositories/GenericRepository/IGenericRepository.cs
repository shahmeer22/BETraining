using System.Linq.Expressions;
using Training.Models;

namespace Training.Repositories.GenericRepository
{
    public interface IGenericRepository<T>
    {
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        T Update(T entity);
        Task<int> SaveChangesAsync();
        IQueryable<T> Query();
        Task<(IQueryable<T>, int)> GetPaginatedResult(IQueryable<T> query, PaginationQueryParams param);
    }
}
